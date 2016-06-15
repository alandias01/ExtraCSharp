using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexFileCreator;
using Data.Slate;
using Data.DTCInstructor;
using Data.DTC;
using WPFUtils;

namespace Pledger.Data
{
    public class PledgeProcessor
    {
        PledgeDA PDA = new PledgeDA();
        OutgoingPledgeOrderDA OPODA = new OutgoingPledgeOrderDA();
        
        TradeCreator tc = new TradeCreator();


        public void PledgeListOfStocks(List<OCCStockPledge> stocksToPledge, bool sendToG1, bool sendToDTC)
        {
            int uniqueid = 0;

            foreach (OCCStockPledge stp in stocksToPledge)
            {
                try
                {
                    PledgeStock(stp, "StockPledger", sendToG1, sendToDTC, uniqueid);
                    uniqueid++;
                }
                catch (Exception ex)
                {
                    Utils.LogError(ex.Message + " " + ex.InnerException.Message);
                }
            }
        }

        public void UnpledgeListOfStocks(List<OCCStockToUnpledge> stocksToUnpledge, bool sendToG1, bool sendToDTC)
        {   
            foreach (OCCStockToUnpledge stu in stocksToUnpledge)
            {
                try
                {
                    UnpledgeStock(stu, "StockPledger", sendToG1, sendToDTC);
                }
                catch (Exception ex)
                {
                    Utils.LogError(ex.Message + " " + ex.InnerException.Message);                    
                }
            }
        }

        private void PledgeStock(OCCStockPledge stp, string source, bool sendToG1, bool sendToDTC, int uniqueid)
        {
            /*            
            If we aren't inserting to DTC and Apex together, then we won't insert into blob.slate.pledge
            In this case, we will have issues with internal unique id, so we set it.
            For OCC, we set it to 199390
            For DTC, internalsourceid = -1   externalsourceid must be unique
            */

            double LoanValue = Convert.ToDouble(stp.QuantityToPledge * stp.RealTimePosition.Price);
            Pledge SlatePledgeItem = PopulateSlatePledgeObject(stp.RealTimePosition, stp.QuantityToPledge, LoanValue, "P", sendToG1, sendToDTC);


            int UniqueIdForEID;
            int internalid;
            int UserReferenceNumber;
            
            if (sendToDTC && sendToG1)
            {
                PDA.Insert(SlatePledgeItem);
            }

            //When not sending to both and only 1 of them, we need to set the ID's
            if (!sendToG1 || !sendToDTC)
            {
                UserReferenceNumber = 199390;       //Current implementation allows only 6 digit number or will crash
                internalid = -1;
                UniqueIdForEID = uniqueid;          //This gets incremented by outside loop
            }
            else
            {
                UniqueIdForEID = SlatePledgeItem.PledgeId;
                internalid = SlatePledgeItem.PledgeId;
                UserReferenceNumber = SlatePledgeItem.PledgeId;
            }

            
            int ReturnedDTCID = 0;
            
            if (sendToDTC)
            {
                ReturnedDTCID = PledgeStockToOCC(UserReferenceNumber, stp.RealTimePosition.cusip, stp.QuantityToPledge, stp.RealTimePosition.ParticipantNum);
            }

            int ReturnedG1ID = 0;
            if (sendToG1)
            {
                string EID = "plg_" + UniqueIdForEID.ToString() + "_" + DateTime.Today.ToString("yyyyMMdd");
                ReturnedG1ID =
                    tc.CreateTrade(source, internalid, GetLegalEntity(stp.RealTimePosition.ParticipantNum),
                    "DTC", EID, "CP", "GC", "OCC", SecurityTypeEnum.CUSIP, stp.RealTimePosition.cusip, DateTime.Today, null, stp.QuantityToPledge,
                    0, .0000001, "USD", 0, Environment.UserName, DateTime.Today, null, 100, 0, null, "USD", DateTime.Today, "DTC0269",
                    null, null, DateTime.Today, stp.RealTimePosition.cusip, true);

                
            }

            if (sendToDTC && sendToG1)
            {
                SlatePledgeItem.G1Id = ReturnedG1ID;
                SlatePledgeItem.DtcId = ReturnedDTCID;
                PDA.Update(SlatePledgeItem);
            }
            
             
        }

        private void UnpledgeStock(OCCStockToUnpledge stu,string source, bool sendToG1, bool sendToDTC)
        {
            List<StartingPositionView> OCCStartingPositions = new List<StartingPositionView>();
            StartingPositionViewDA SPVDA = new StartingPositionViewDA();
            SPVDA.Load(OCCStartingPositions, "OCC", stu.RealTimePosition.ParticipantNum, stu.RealTimePosition.cusip);

            //If we don't have a position in it, we won't do anything.  Try to return a message saying so
            if (OCCStartingPositions.Count > 0)
            {
                Pledge SlateUnpledgeItem = PopulateSlatePledgeObject(stu.RealTimePosition,stu.SharesToUnpledge,0, "U", sendToG1, sendToDTC);
                
                PDA.Insert(SlateUnpledgeItem);

                int ReturnedG1ID = CreateTheReturnForTheUnpledge(OCCStartingPositions, source, SlateUnpledgeItem.PledgeId, stu.SharesToUnpledge, stu.RealTimePosition.ParticipantNum);

                int ReturnedDTCID = UnpledgeStockFromOCC(SlateUnpledgeItem.PledgeId, stu.RealTimePosition.cusip, stu.SharesToUnpledge, stu.RealTimePosition.ParticipantNum);

                SlateUnpledgeItem.G1Id = ReturnedG1ID;
                SlateUnpledgeItem.DtcId = ReturnedDTCID;
                PDA.Update(SlateUnpledgeItem);
            }
        }

        private Pledge PopulateSlatePledgeObject(spAlanGetRealTimePositions_Pledger_Result stu, int shares, double LoanValue, string pledgeType, bool sendToG1, bool sendToDTC)
        {
            Pledge p = new Pledge();
            p.PledgeType = pledgeType;
            p.Source = "StockPledger";
            p.DateEntered = DateTime.Now;
            p.EnteredBy = Environment.UserName;
            p.Processed = false;
            p.SendToG1 = sendToG1;
            p.SendToDtc = sendToDTC;
            p.ClearingNo = stu.ParticipantNum;
            p.Cusip = stu.cusip;
            p.Ticker = stu.Ticker;
            p.Quantity = shares;
            p.LoanValue = LoanValue;
            p.Pledgor = stu.ParticipantNum.TrimStart("0".ToCharArray());
            p.Pledgee = "981";
            p.DtcStatusCode = "";

            return p;
        }

        private int CreateTheReturnForTheUnpledge(List<StartingPositionView> OCCStartingPositions, string source, int PledgeIdFromLocalTable,  int SharesToUnpledge, string clearingNo)
        {
            //test
            /*
            StartingPositionView ss = new StartingPositionView();
            ss.TradeRef = "123";
            ss.Quantity = 1;
            ss.Cusip = "123456789";
            OCCStartingPositions.Add(ss);
            */

            int g1=0;
            int sharesUnpledged = 0;
            int sharesToUnpledgeForThisPosition = 0;

            

            foreach (StartingPositionView SPV in OCCStartingPositions)
            {
                if (sharesUnpledged < SharesToUnpledge)
                {
                    if (Math.Abs(SPV.Quantity.Value) >= SharesToUnpledge - sharesUnpledged)
                    {
                        sharesToUnpledgeForThisPosition = SharesToUnpledge - sharesUnpledged;
                    }
                    else
                    {
                        sharesToUnpledgeForThisPosition = (int)Math.Abs(SPV.Quantity.Value);
                    }

                    sharesUnpledged += sharesToUnpledgeForThisPosition;

                    string EID = "plg_" + PledgeIdFromLocalTable.ToString() + "_" + DateTime.Today.ToString("yyyyMMdd");
                    g1 = tc.CreateMovement(Environment.UserName, source, PledgeIdFromLocalTable, GetLegalEntity(clearingNo), SPV.TradeRef, DateTime.Today, sharesToUnpledgeForThisPosition, 0, EID);
 
                }
 
            }

            return g1;
        }

        private string GetLegalEntity(string clearingNo)
        {
            switch (clearingNo)
            {
                case "0269": return "MS0269";
                case "5239": return "MS5239";
                case "5289": return "MS5289";
                default:
                    return null;
            }
        }

        private int UnpledgeStockFromOCC(int PledgeIdFromLocalTable, string CUSIP, int SharesToUnpledge, string clearingNo)
        {
            OutgoingPledgeOrder OPO = CreateBasePledge(PledgeIdFromLocalTable, "27", CUSIP, SharesToUnpledge, clearingNo);
                        
            OPO.Pledgee = "";
            OPO.LoanDate = "";
            OPO.OccParticipantNumber = clearingNo.PadLeft(8, '0');
            OPO.OccNumber = "981";

            OPODA.Insert(OPO);

            return OPO.Id;
        }

        private int PledgeStockToOCC(int PledgeIdFromLocalTable, string CUSIP, int SharesToPledge, string clearingNo)
        {
            OutgoingPledgeOrder OPO = CreateBasePledge(PledgeIdFromLocalTable, "21", CUSIP, SharesToPledge, clearingNo);
            
            OPO.PledgePurpose = "1";
            OPO.HypothecationCode = "1";

            OPODA.Insert(OPO);

            return OPO.Id;
        }
        
        private OutgoingPledgeOrder CreateBasePledge(int PledgeIdFromLocalTable,string RecordType, string CUSIP, int SharesToUnpledge, string clearingNo)
        {
            OutgoingPledgeOrder OPO = new OutgoingPledgeOrder();

            OPO.DateEntered = DateTime.Now;
            OPO.EnteredBy = Environment.UserName;
            OPO.FeedbackIndicator = "";
            OPO.ProductionIndicator = "P";
            OPO.RecType = "COLC02";
            OPO.RecordSuffix = "01";
            OPO.VersionNumber = "01";
            OPO.UserReferenceNumber = PledgeIdFromLocalTable.ToString().Substring(0, 6);    //Reason we do this is because the table only allows up to 6 characters
            OPO.Adressee = "G0000346";
            OPO.RecordType = RecordType;
            OPO.Pledgor = clearingNo.PadLeft(8, '0');
            OPO.Pledgee = "00000981";
            OPO.Cusip = CUSIP;
            OPO.ShareQuantity = SharesToUnpledge.ToString().PadLeft(9, '0');
            OPO.LoanDate = "19730320";
            OPO.LoanValueWhole = "";
            OPO.LoanValueDecimal = "";
            OPO.PledgePurpose = "";
            OPO.ReleaseType = "";
            OPO.HypothecationCode = "";
            OPO.PreventPendIndicator = "";
            OPO.CnsIndicator = "";
            OPO.IpoIndicator = "";
            OPO.PtaIndicator = "";
            OPO.FederalReservePurpose = "";
            OPO.OccParticipantNumber = "";
            OPO.BankAbaNumber = "";
            OPO.OccNumber = "";
            OPO.OccClearingGroupId = "";
            OPO.OccClearingMemberNumber = clearingNo.PadLeft(5, '0');
            OPO.OccAccountType = "F";
            OPO.OccAccountId = "";
            OPO.OccCollateralType = "VS";
            OPO.OccOptionSymbol="";
            OPO.OccExpirationYear="";
            OPO.OccExpirationMonth="";
            OPO.OccExpirationDay="";
            OPO.OccOptionType="";
            OPO.OccOptionStrike="";
            OPO.OccOptionStrikeDecimal = "";
            OPO.Blank = "";
            OPO.CrossReferenceNumber = PledgeIdFromLocalTable.ToString();
            OPO.CustomerAccountNumber="";
            OPO.OccFiller="";
            OPO.DtccFiller = "";
            
            return OPO; 
        }


    }
}
