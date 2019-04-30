using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoniz.Common.Data.DataConvertor;
using Shoniz.Common.Data.SqlServer;

namespace TestUtility
{
    public class Program
    {
        public static SqlDataReader reader = null;

        private static void Main(string[] args)
        {
            //var conn =
            //    new SqlConnection(
            //        "Data Source=HRMS;Initial Catalog=laboratory;Persist Security Info=True;User ID=sa;Password=S@334sai334@Sxxx");
            //var command = new SqlCommand("sp_GetStuffRegistration2ORMTest", conn);
            //command.CommandType = CommandType.StoredProcedure;
            //command.Parameters.Add("@mainTestRegistrationId", 640);
            //conn.Open();
            //reader = command.ExecuteReader();
            //var dt = new DataTable();
            //dt.Load(reader);
            //var list = new DataTableToList().Convert<StuffTestRegisterVM>(dt);


            //var tableBased = new TableBasedSp("RunSp").GetFirstTableOfData("uspGetElementData",
            //    "Data Source=FROSH;Initial Catalog=UM;User ID=saleadmin;Password=H2389*x;",
            //    new Dictionary<string, string>
            //    {
            //        {"ProgramId", "2"},
            //        {"ElementId", "36"}
            //    }
            //    );

            //var t2 = new TableBasedSp("RunSp").GetAllTablesOfData("uspGetElementData",
            //    "Data Source=FROSH;Initial Catalog=UM;User ID=saleadmin;Password=H2389*x;",
            //    new Dictionary<string, string>
            //    {
            //        {"ProgramId", "2"},
            //        {"ElementId", "36"}
            //    }
            //    );

            //var t3 = new TableBasedSp("RunSp").RunSp("uspGetElementData",
            //    "Data Source=FROSH;Initial Catalog=UM;User ID=saleadmin;Password=H2389*x;",
            //    new Dictionary<string, string>
            //    {
            //        {"ProgramId", "2"},
            //        {"ElementId", "36"}
            //    }
            //    );

            //var list = new DataTableToList().Convert<MenuVm>(tableBased);
            //var list2 = new DataTableToList().Convert<MenuVm>(t2.Tables[0]);

            try
            {
                



                var db = new TableBasedSp("Business.uspRunSp");
            DataSet ds = db.GetAllTablesOfData("[Business].[uspGetWorkorders]",
                @"Data Source=cmms;Initial Catalog=WorkorderManagement;Integrated Security=True",
                new Dictionary<string, string>
                {
                    {"_CD", "2016-02-13 09:36:05.677"},
                    {"_Username", "890012"},
                    {"_Password", "123"},
                    {"_ExecutingVersion", "1.3.1.0"},
                    {"_RequestId", Guid.NewGuid().ToString()},
                    {"ReportSectionId", "1"},
                    {"PropertyNames", "SelectedRequesterUnits;"},
                    {"PropertyValues", "1,2;"},
                    {"WorkorderIds", "342"},
                    {"CheckId", "2"},
                    {"CheckResult", "p0"},
                    {"WorkorderDetailIds", "234,239,240,241"},
                    {"ScheduleStart", "2015-06-27 10:00:00.000"},
                    {"ScheduleFinish", "2015-07-01 09:00:00.000"},
                    {"WorkorderId", "330"},
                    {"Modes", "0"},
                    {"FromDate", "2010-06-23 14:25:44.267"},
                    {"ToDate", "2020-06-23 14:25:44.267"},
                    {"PreferredRole", "-1"},
                    {"Requester", "930682"},
                    {"RequestDate", "2015-09-05 08:53:37.315"},
                    {"NeedDate", "2015-09-06 08:53:37.315"},
                    {"WorkUnitId", "147"},
                    {"RequesterUnitId", "126"},
                    {"Description", ""},
                    {"DeviceCode", ""},
                    {
                        "WorkorderDetails",
                        "<DocumentElement>  <WorkorderDetails>    <OrderToPerform>0</OrderToPerform>    <JobToDo>تست درخواست</JobToDo>  </WorkorderDetails></DocumentElement>"
                    }

                });

                var a = 1;
            }
            catch (Shoniz.Common.Data.SqlServer.DataException ex)
            {

            }













        }



        public TestUtility.Program.StuffTestRegisterVM Convert(DataRow dataRow)
        {
            var _StuffTestRegisterVM = new TestUtility.Program.StuffTestRegisterVM();
            if (dataRow.Table.Columns.Count > 0)
            {
                if (dataRow.Table.Columns.IndexOf("StoreCode") > -1)
                {
                    _StuffTestRegisterVM.StoreCode = (String)(!DBNull.Value.Equals(dataRow["StoreCode"]) ? dataRow["StoreCode"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("BatchCode") > -1)
                {
                    _StuffTestRegisterVM.BatchCode = (String)(!DBNull.Value.Equals(dataRow["BatchCode"]) ? dataRow["BatchCode"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("Mount") > -1)
                {
                    _StuffTestRegisterVM.Mount = !DBNull.Value.Equals(dataRow["Mount"]) ? Double.Parse(dataRow["Mount"].ToString()) : default(Double);
                }
                if (dataRow.Table.Columns.IndexOf("CompanyId") > -1)
                {
                    _StuffTestRegisterVM.CompanyId = !DBNull.Value.Equals(dataRow["CompanyId"]) ? Int32.Parse(dataRow["CompanyId"].ToString()) : default(Int32);
                }
                if (dataRow.Table.Columns.IndexOf("BrandId") > -1)
                {
                    _StuffTestRegisterVM.BrandId = !DBNull.Value.Equals(dataRow["BrandId"]) ? Int32.Parse(dataRow["BrandId"].ToString()) : default(Int32);
                }
                if (dataRow.Table.Columns.IndexOf("ProduceDate") > -1)
                {
                    _StuffTestRegisterVM.ProduceDate = (String)(!DBNull.Value.Equals(dataRow["ProduceDate"]) ? dataRow["ProduceDate"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("ExpireDate") > -1)
                {
                    _StuffTestRegisterVM.ExpireDate = (String)(!DBNull.Value.Equals(dataRow["ExpireDate"]) ? dataRow["ExpireDate"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("PackingTypeId") > -1)
                {
                    _StuffTestRegisterVM.PackingTypeId = !DBNull.Value.Equals(dataRow["PackingTypeId"]) ? Int32.Parse(dataRow["PackingTypeId"].ToString()) : default(Int32);
                }
                if (dataRow.Table.Columns.IndexOf("MountType") > -1)
                {
                    _StuffTestRegisterVM.MountType = !DBNull.Value.Equals(dataRow["MountType"]) ? Int32.Parse(dataRow["MountType"].ToString()) : default(Int32);
                }
                if (dataRow.Table.Columns.IndexOf("TestTypeIdOfPage") > -1)
                {
                    _StuffTestRegisterVM.TestTypeIdOfPage = !DBNull.Value.Equals(dataRow["TestTypeIdOfPage"]) ? Int32.Parse(dataRow["TestTypeIdOfPage"].ToString()) : default(Int32);
                }
                if (dataRow.Table.Columns.IndexOf("Id") > -1)
                {
                    _StuffTestRegisterVM.Id = !DBNull.Value.Equals(dataRow["Id"]) ? Int32.Parse(dataRow["Id"].ToString()) : default(Int32);
                }
                if (dataRow.Table.Columns.IndexOf("StuffId") > -1)
                {
                    _StuffTestRegisterVM.StuffId = !DBNull.Value.Equals(dataRow["StuffId"]) ? Int32.Parse(dataRow["StuffId"].ToString()) : default(Int32);
                }
                if (dataRow.Table.Columns.IndexOf("SampleCode") > -1)
                {
                    _StuffTestRegisterVM.SampleCode = (String)(!DBNull.Value.Equals(dataRow["SampleCode"]) ? dataRow["SampleCode"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("RegisterCode") > -1)
                {
                    _StuffTestRegisterVM.RegisterCode = (String)(!DBNull.Value.Equals(dataRow["RegisterCode"]) ? dataRow["RegisterCode"].ToString() : "");
                }
            }
            _StuffTestRegisterVM.CommonMainTestRegisterVM = new TestUtility.Program.MainTestRegisterVM();
            if (dataRow.Table.Columns.Count > 0)
            {
                if (dataRow.Table.Columns.IndexOf("TestTypeId") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.TestTypeId = !DBNull.Value.Equals(dataRow["TestTypeId"]) ? Int32.Parse(dataRow["TestTypeId"].ToString()) : default(Int32);
                }
                if (dataRow.Table.Columns.IndexOf("ItemType") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.ItemType = (ItemTypesEnum)(dataRow["ItemType"]);
                }
                if (dataRow.Table.Columns.IndexOf("RequirementPerson") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.RequirementPerson = (String)(!DBNull.Value.Equals(dataRow["RequirementPerson"]) ? dataRow["RequirementPerson"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("Sampler") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.Sampler = (String)(!DBNull.Value.Equals(dataRow["Sampler"]) ? dataRow["Sampler"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("TesterCode") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.TesterCode = !DBNull.Value.Equals(dataRow["TesterCode"]) ? Int32.Parse(dataRow["TesterCode"].ToString()) : default(Int32);
                }
                if (dataRow.Table.Columns.IndexOf("ConfirmorCode") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.ConfirmorCode = !DBNull.Value.Equals(dataRow["ConfirmorCode"]) ? Int32.Parse(dataRow["ConfirmorCode"].ToString()) : default(Int32);
                }
                if (dataRow.Table.Columns.IndexOf("SamplingLocationId") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.SamplingLocationId = (String)(!DBNull.Value.Equals(dataRow["SamplingLocationId"]) ? dataRow["SamplingLocationId"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("SampleName") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.SampleName = (String)(!DBNull.Value.Equals(dataRow["SampleName"]) ? dataRow["SampleName"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("TestDate") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.TestDate = (String)(!DBNull.Value.Equals(dataRow["TestDate"]) ? dataRow["TestDate"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("AdmissionDate") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.AdmissionDate = (String)(!DBNull.Value.Equals(dataRow["AdmissionDate"]) ? dataRow["AdmissionDate"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("ResultDate") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.ResultDate = (String)(!DBNull.Value.Equals(dataRow["ResultDate"]) ? dataRow["ResultDate"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("ReportDate") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.ReportDate = (String)(!DBNull.Value.Equals(dataRow["ReportDate"]) ? dataRow["ReportDate"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("TestBeginDate") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.TestBeginDate = (String)(!DBNull.Value.Equals(dataRow["TestBeginDate"]) ? dataRow["TestBeginDate"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("SampleGettingDate") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.SampleGettingDate = (String)(!DBNull.Value.Equals(dataRow["SampleGettingDate"]) ? dataRow["SampleGettingDate"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("ReportReformDate") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.ReportReformDate = (String)(!DBNull.Value.Equals(dataRow["ReportReformDate"]) ? dataRow["ReportReformDate"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("SamplingDate") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.SamplingDate = (String)(!DBNull.Value.Equals(dataRow["SamplingDate"]) ? dataRow["SamplingDate"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("Referencer") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.Referencer = (String)(!DBNull.Value.Equals(dataRow["Referencer"]) ? dataRow["Referencer"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("ReferenceDate") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.ReferenceDate = (String)(!DBNull.Value.Equals(dataRow["ReferenceDate"]) ? dataRow["ReferenceDate"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("ReferenceNo") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.ReferenceNo = (String)(!DBNull.Value.Equals(dataRow["ReferenceNo"]) ? dataRow["ReferenceNo"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("Description") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.Description = (String)(!DBNull.Value.Equals(dataRow["Description"]) ? dataRow["Description"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("Address") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.Address = (String)(!DBNull.Value.Equals(dataRow["Address"]) ? dataRow["Address"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("Tel") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.Tel = (String)(!DBNull.Value.Equals(dataRow["Tel"]) ? dataRow["Tel"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("MessageType") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.MessageType = !DBNull.Value.Equals(dataRow["MessageType"]) ? Int32.Parse(dataRow["MessageType"].ToString()) : default(Int32);
                }
                if (dataRow.Table.Columns.IndexOf("IsDeleted") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.IsDeleted = !DBNull.Value.Equals(dataRow["IsDeleted"]) ? Boolean.Parse(dataRow["IsDeleted"].ToString()) : default(Boolean);
                }
                if (dataRow.Table.Columns.IndexOf("UserId") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.UserId = !DBNull.Value.Equals(dataRow["UserId"]) ? Int32.Parse(dataRow["UserId"].ToString()) : default(Int32);
                }
                if (dataRow.Table.Columns.IndexOf("AnswerComment") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.AnswerComment = (String)(!DBNull.Value.Equals(dataRow["AnswerComment"]) ? dataRow["AnswerComment"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("IsPrivateExam") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.IsPrivateExam = !DBNull.Value.Equals(dataRow["IsPrivateExam"]) ? Boolean.Parse(dataRow["IsPrivateExam"].ToString()) : default(Boolean);
                }
                if (dataRow.Table.Columns.IndexOf("Id") > -1)
                {
                    _StuffTestRegisterVM.CommonMainTestRegisterVM.Id = !DBNull.Value.Equals(dataRow["Id"]) ? Int32.Parse(dataRow["Id"].ToString()) : default(Int32);
                }
            }
            _StuffTestRegisterVM.PersonTestRegisterVM = new TestUtility.Program.PersonTestRegisterVM();
            if (dataRow.Table.Columns.Count > 0)
            {
                if (dataRow.Table.Columns.IndexOf("PersonTestRegisterId") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.PersonTestRegisterId = !DBNull.Value.Equals(dataRow["PersonTestRegisterId"]) ? Int32.Parse(dataRow["PersonTestRegisterId"].ToString()) : default(Int32);
                }
                if (dataRow.Table.Columns.IndexOf("PersonalCode") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.PersonalCode = !DBNull.Value.Equals(dataRow["PersonalCode"]) ? Int32.Parse(dataRow["PersonalCode"].ToString()) : default(Int32);
                }
                if (dataRow.Table.Columns.IndexOf("WorkingLocationCode") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.WorkingLocationCode = (String)(!DBNull.Value.Equals(dataRow["WorkingLocationCode"]) ? dataRow["WorkingLocationCode"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("GrowLocation") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.GrowLocation = (String)(!DBNull.Value.Equals(dataRow["GrowLocation"]) ? dataRow["GrowLocation"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("ImportNote") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.ImportNote = (String)(!DBNull.Value.Equals(dataRow["ImportNote"]) ? dataRow["ImportNote"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("Result") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.Result = (String)(!DBNull.Value.Equals(dataRow["Result"]) ? dataRow["Result"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("Mount") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.Mount = !DBNull.Value.Equals(dataRow["Mount"]) ? Double.Parse(dataRow["Mount"].ToString()) : default(Double);
                }
                if (dataRow.Table.Columns.IndexOf("CompanyId") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CompanyId = !DBNull.Value.Equals(dataRow["CompanyId"]) ? Double.Parse(dataRow["CompanyId"].ToString()) : default(Double);
                }
            }
            _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM = new TestUtility.Program.MainTestRegisterVM();
            if (dataRow.Table.Columns.Count > 0)
            {
                if (dataRow.Table.Columns.IndexOf("TestTypeId") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.TestTypeId = !DBNull.Value.Equals(dataRow["TestTypeId"]) ? Int32.Parse(dataRow["TestTypeId"].ToString()) : default(Int32);
                }
                if (dataRow.Table.Columns.IndexOf("ItemType") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.ItemType = (ItemTypesEnum)(dataRow["ItemType"]);
                }
                if (dataRow.Table.Columns.IndexOf("RequirementPerson") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.RequirementPerson = (String)(!DBNull.Value.Equals(dataRow["RequirementPerson"]) ? dataRow["RequirementPerson"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("Sampler") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.Sampler = (String)(!DBNull.Value.Equals(dataRow["Sampler"]) ? dataRow["Sampler"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("TesterCode") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.TesterCode = !DBNull.Value.Equals(dataRow["TesterCode"]) ? Int32.Parse(dataRow["TesterCode"].ToString()) : default(Int32);
                }
                if (dataRow.Table.Columns.IndexOf("ConfirmorCode") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.ConfirmorCode = !DBNull.Value.Equals(dataRow["ConfirmorCode"]) ? Int32.Parse(dataRow["ConfirmorCode"].ToString()) : default(Int32);
                }
                if (dataRow.Table.Columns.IndexOf("SamplingLocationId") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.SamplingLocationId = (String)(!DBNull.Value.Equals(dataRow["SamplingLocationId"]) ? dataRow["SamplingLocationId"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("SampleName") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.SampleName = (String)(!DBNull.Value.Equals(dataRow["SampleName"]) ? dataRow["SampleName"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("TestDate") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.TestDate = (String)(!DBNull.Value.Equals(dataRow["TestDate"]) ? dataRow["TestDate"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("AdmissionDate") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.AdmissionDate = (String)(!DBNull.Value.Equals(dataRow["AdmissionDate"]) ? dataRow["AdmissionDate"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("ResultDate") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.ResultDate = (String)(!DBNull.Value.Equals(dataRow["ResultDate"]) ? dataRow["ResultDate"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("ReportDate") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.ReportDate = (String)(!DBNull.Value.Equals(dataRow["ReportDate"]) ? dataRow["ReportDate"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("TestBeginDate") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.TestBeginDate = (String)(!DBNull.Value.Equals(dataRow["TestBeginDate"]) ? dataRow["TestBeginDate"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("SampleGettingDate") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.SampleGettingDate = (String)(!DBNull.Value.Equals(dataRow["SampleGettingDate"]) ? dataRow["SampleGettingDate"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("ReportReformDate") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.ReportReformDate = (String)(!DBNull.Value.Equals(dataRow["ReportReformDate"]) ? dataRow["ReportReformDate"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("SamplingDate") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.SamplingDate = (String)(!DBNull.Value.Equals(dataRow["SamplingDate"]) ? dataRow["SamplingDate"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("Referencer") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.Referencer = (String)(!DBNull.Value.Equals(dataRow["Referencer"]) ? dataRow["Referencer"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("ReferenceDate") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.ReferenceDate = (String)(!DBNull.Value.Equals(dataRow["ReferenceDate"]) ? dataRow["ReferenceDate"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("ReferenceNo") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.ReferenceNo = (String)(!DBNull.Value.Equals(dataRow["ReferenceNo"]) ? dataRow["ReferenceNo"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("Description") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.Description = (String)(!DBNull.Value.Equals(dataRow["Description"]) ? dataRow["Description"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("Address") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.Address = (String)(!DBNull.Value.Equals(dataRow["Address"]) ? dataRow["Address"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("Tel") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.Tel = (String)(!DBNull.Value.Equals(dataRow["Tel"]) ? dataRow["Tel"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("MessageType") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.MessageType = !DBNull.Value.Equals(dataRow["MessageType"]) ? Int32.Parse(dataRow["MessageType"].ToString()) : default(Int32);
                }
                if (dataRow.Table.Columns.IndexOf("IsDeleted") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.IsDeleted = !DBNull.Value.Equals(dataRow["IsDeleted"]) ? Boolean.Parse(dataRow["IsDeleted"].ToString()) : default(Boolean);
                }
                if (dataRow.Table.Columns.IndexOf("UserId") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.UserId = !DBNull.Value.Equals(dataRow["UserId"]) ? Int32.Parse(dataRow["UserId"].ToString()) : default(Int32);
                }
                if (dataRow.Table.Columns.IndexOf("AnswerComment") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.AnswerComment = (String)(!DBNull.Value.Equals(dataRow["AnswerComment"]) ? dataRow["AnswerComment"].ToString() : "");
                }
                if (dataRow.Table.Columns.IndexOf("IsPrivateExam") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.IsPrivateExam = !DBNull.Value.Equals(dataRow["IsPrivateExam"]) ? Boolean.Parse(dataRow["IsPrivateExam"].ToString()) : default(Boolean);
                }
                if (dataRow.Table.Columns.IndexOf("Id") > -1)
                {
                    _StuffTestRegisterVM.PersonTestRegisterVM.CommonMainTestRegisterVM.Id = !DBNull.Value.Equals(dataRow["Id"]) ? Int32.Parse(dataRow["Id"].ToString()) : default(Int32);
                }
            }
            return _StuffTestRegisterVM;
        }
       

        public enum ItemTypesEnum
        {
            محصولات = 1,
            اولیه = 2,
            درخواست = 3,
            پرسنل = 4
        }

        public class MenuVm
        {
            public int UserId { get; set; }
            public int MenuId { get; set; }
            public string MenuName { get; set; }
            public short? ParentMenuId { get; set; }
            public decimal MenuOrder { get; set; }
            public string Url { get; set; }
            public string PageName { get; set; }
            public string MenuIcon { get; set; }
            public decimal Order { get; set; }
            public int ParentId { get; set; }
        }

        public class MainTestRegisterVM
        {

            public int Id;

            [DisplayName("نوع آزمایش")]
            public int TestTypeId { get; set; }

            [DisplayName("نوع مورد آزمایشی")]
            public ItemTypesEnum ItemType { get; set; } //محصولات-اولیه-درخواستها-پرسنل

            //[Required(ErrorMessage = "تکمیل این فیلد الزامیست")]
            [DisplayName("درخواست کننده")]
            public string RequirementPerson { get; set; }

            //[Required(ErrorMessage = "تکمیل این فیلد الزامیست")]
            [DisplayName("نمونه بردار")]
            public string Sampler { get; set; }

            [DisplayName("آزمایش کننده")]
            public int TesterCode { get; set; }

            [DisplayName("تایید کننده")]
            public int ConfirmorCode { get; set; }

            [DisplayName("محل نمونه برداری")]
            public string SamplingLocationId { get; set; }

            [DisplayName("نمونه تحت آزمون")]
            public string SampleName { get; set; }

            //[Required(ErrorMessage = "تکمیل این فیلد الزامیست")]
            [DisplayName("تاریخ آزمون")]
            public string TestDate { get; set; }

            //[Required(ErrorMessage = "تکمیل این فیلد الزامیست")]
            [DisplayName("تاریخ پذیرش")]
            public string AdmissionDate { get; set; }

            [DisplayName("تاریخ جوابدهی")]
            public string ResultDate { get; set; }

            [DisplayName("تاریخ گزارش")]
            public string ReportDate { get; set; }

            //[Required(ErrorMessage = "تکمیل این فیلد الزامیست")]
            [DisplayName("تاریخ شروع آزمون")]
            public string TestBeginDate { get; set; }

            //[Required(ErrorMessage = "تکمیل این فیلد الزامیست")]
            [DisplayName("تاریخ اخذ نمونه")]
            public string SampleGettingDate { get; set; }

            [DisplayName("تاریخ آخرین اصلاح")]
            public string ReportReformDate { get; set; }

            //[Required(ErrorMessage = "تکمیل این فیلد الزامیست")]
            //[DataType(DataType.DateTime, ErrorMessage = "فرمت تاریخ")]
            [DisplayName("تاریخ نمونه برداری")]
            public string SamplingDate { get; set; }

            [DisplayName("ارجاع دهنده")]
            public string Referencer { get; set; }

            [DisplayName("تاریخ ارجاع")]
            public string ReferenceDate { get; set; }

            [DisplayName("شماره ارجاع")]
            public string ReferenceNo { get; set; }

            [DisplayName("توضیحات")]
            public string Description { get; set; }

            [DisplayName("آدرس")]
            public string Address { get; set; }

            [DisplayName("شماره تماس")]
            public string Tel { get; set; }

            public int MessageType { get; set; }

            public bool IsDeleted { get; set; }

            public int UserId { get; set; }



            public string AnswerComment { get; set; }

            [DisplayName("نمونه خارجی")]
            public bool IsPrivateExam { get; set; }
        }



        public class StuffTestRegisterVM
        {
            public StuffTestRegisterVM()
            {
                CommonMainTestRegisterVM = new MainTestRegisterVM();
            }



            public int Id;


            public int StuffId;

            //[Required(ErrorMessage = "این فیلد الزامیست")]
            //[StringLength(15, MinimumLength = 1)]

            public string SampleCode;

            //[StringLength(15, MinimumLength = 1)]

            public string RegisterCode;

            //[StringLength(15, MinimumLength = 1)]
            [DisplayName("شناسه")]
            public string StoreCode { get; set; }

            //[StringLength(15, MinimumLength = 1)]
            [DisplayName("سری ساخت/بچ")]
            public string BatchCode { get; set; }

            //[Required(ErrorMessage = "این فیلد الزامیست")]
            //[Range(0, 999999.999, ErrorMessage = "فقط عدد می توانید وارد نمایید")]
            [DisplayName("مقدار")]
            public double Mount { get; set; }

            [DisplayName("شرکت")]
            public int CompanyId { get; set; }

            //[StringLength(25, MinimumLength = 1)]
            [DisplayName("مارک تجاری")]
            public int BrandId { get; set; }

            //[DataType(DataType.Date)]
            [DisplayName("تاریخ تولید")]
            public string ProduceDate { get; set; }

            //[DataType(DataType.Date)]
            [DisplayName("تاریخ انقضاء")]
            public string ExpireDate { get; set; }

            [DisplayName("نوع بسته بندی")]
            public int PackingTypeId { get; set; }

            [DisplayName("واحد مقدار")]
            public int MountType { get; set; }


            public MainTestRegisterVM CommonMainTestRegisterVM { get; set; }

            public PersonTestRegisterVM PersonTestRegisterVM { get; set; }
            public int TestTypeIdOfPage { get; set; }
        }

        public class PersonTestRegisterVM
        {
            public PersonTestRegisterVM()
            {
                CommonMainTestRegisterVM = new MainTestRegisterVM();
            }

            [DisplayName("کد ثبت")]
            public int PersonTestRegisterId { get; set; }

            //[Required(ErrorMessage = "این فیلد الزامیست")]
            //[DisplayName("نام نمونه")]
            //public string SampleName { get; set; }
            [DisplayName("کد پرسنلی")]
            public int PersonalCode { get; set; }

            [DisplayName("محل کار")]
            public string WorkingLocationCode { get; set; }

            //[Required(ErrorMessage = "این فیلد الزامیست")]
            [DisplayName("محیط کشت")]
            public string GrowLocation { get; set; }

            [DisplayName("نکات مهم")]
            public string ImportNote { get; set; }

            [DisplayName("نتیجه")]
            public string Result { get; set; }

            public double Mount { get; set; }

            public double CompanyId { get; set; }

            public MainTestRegisterVM CommonMainTestRegisterVM { get; set; }
        }

        public class TestRegisterDetailVM
        {
            public int DetailId { get; set; }
            public int MainTestRegisterId { get; set; }
            public int TestId { get; set; }
            public int? Cost { get; set; }
            public string Result { get; set; }
        }
    }
}
