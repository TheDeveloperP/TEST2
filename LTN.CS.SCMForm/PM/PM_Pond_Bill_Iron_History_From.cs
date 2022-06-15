using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using LTN.CS.Base;
using LTN.CS.SCMService.PM.Interface;
using System.Collections;
using LTN.CS.SCMForm.Common;
using LTN.CS.Core.Helper;
using LTN.CS.SCMEntities.PM;

namespace LTN.CS.SCMForm.PM
{
    public partial class PM_Pond_Bill_Iron_History_From : CoreForm
    {
        public IPM_Pond_Bill_Iron_HistoryService MainService { get; set; }
        private int selectMainId { get; set; }
        private bool queryMain { get; set; }
        private int selectMainRowNum { get; set; }
        public PM_Pond_Bill_Iron_History_From()
        {
            InitializeComponent();
        }
        private void gToolStripButton1_Click(object sender, EventArgs e)
        {
            Query();
        }
        //查询
        private void Query(bool isFirst = false)
        {
            try
            {
                int selectLeftIdOld = selectMainId;
                queryMain = false;
                Hashtable ht = new Hashtable();
                ht.Add("PlanNo", txt_PlanNo.Text.Trim());
                ht.Add("WgtlistNo", txt_WgtlistNo.Text.Trim());
                ht.Add("HeatNo", txt_HeatNo.Text.Trim());
                ht.Add("TankNo", txt_TankNo.Text.Trim());
                if (!string.IsNullOrEmpty(date_StartTime.Text) && !string.IsNullOrEmpty(date_EndTime.Text))
                {
                    ht.Add("StartTime", CommonHelper.TimeToStr14(MyDateTimeHelper.ConvertToDateTimeDefaultNow(date_StartTime.Text)));
                    ht.Add("EndTime", CommonHelper.TimeToStr14(MyDateTimeHelper.ConvertToDateTimeDefaultNow(date_EndTime.Text)));

                }
                var result = MainService.ExecuteDB_QueryPM_Pond_Bill_Iron_HistoryByHashtable(ht);
                gridControl1.DataSource = result;
                if (!isFirst)
                {
                    selectMainId = selectLeftIdOld;
                    SetMainFocusRow();
                }
                queryMain = true;
            }
            catch (Exception)
            {
            }
        }
        private void SetMainFocusRow()
        {
            int rowcount = gridView1.RowCount;
            bool isFocused = false;
            if (selectMainId != 0)
            {
                for (int i = 0; i < rowcount; i++)
                {
                    PM_Pond_Bill_Iron_History group = gridView1.GetRow(i) as PM_Pond_Bill_Iron_History;
                    if (group.IntId == selectMainId)
                    {
                        gridView1.FocusedRowHandle = i;
                        selectMainRowNum = i;
                        isFocused = true;
                        break;
                    }
                }
            }
            if (!isFocused)
            {
                if (rowcount - 1 < selectMainRowNum)
                {
                    gridView1.FocusedRowHandle = rowcount - 1;
                    selectMainRowNum = rowcount - 1;
                }
                else
                {
                    gridView1.FocusedRowHandle = selectMainRowNum;
                }
            }
        }
        //加载页面日期初始化
        private void PM_Pond_Bill_Iron_History_Form_Load(object sender, EventArgs e)
        {
            date_EndTime.EditValue = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
            date_StartTime.EditValue = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd 00:00:00");
            Query();
        }
        //C_UPDATECOLUMNS用于记录更新的字段
        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            var item = gridView1.GetRow(e.RowHandle) as PM_Pond_Bill_Iron_History;
            if (item == null)
                return;
            if (!String.IsNullOrEmpty(item.UpDateColumns))
            {
                //if (item.UpDateColumns == "作废")
                //{
                //    e.Appearance.BackColor = Color.Red;
                //}
                string[] columns = item.UpDateColumns.Split('‖', '&');
                if (columns != null && columns.Contains(e.Column.FieldName))
                {
                    e.Appearance.BackColor = Color.Red;

                }
            }
        }

        //修改信息消息提示
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            string[] Unwanted = { "IntId","ToDeptNo","NetWgt1","SlagNum","NetWgt2",
                                  "GrossWgtSiteNo","TareWgtSiteNo","TrainGroupGross","TrainGroupTare",
                                  "CReserve1","CReserve2","CReserve3","IReserve1","IReserve2","IReserve3",
                                  "CReserveFlag1","CReserveFlag2","CReserveFlag3","IReserveFlag1",
                                  "IReserveFlag2","IReserveFlag1","DataFlag.EntityValue","BillStatus.IntValue",
                                  "UpLoadStatus","PlanStatus","UpdateUser","UpdateTime"
                                }; //提示-过滤不用字段
            string str = null;
            var item = gridView1.GetFocusedRow() as PM_Pond_Bill_Iron_History;
            if (item == null)
                return;
            if (!String.IsNullOrEmpty(item.UpDateColumns))
            {
                string[] columns = item.UpDateColumns.Split('‖', '&');
                if (columns != null)
                {
                    for (int i = 0; i < columns.Length - 1; i += 2)//偶数a为字段名称 a+1单数为对应值 
                    {
                        if (Unwanted.Contains(columns[i]))
                        {
                            continue;
                        }
                        else
                        {
                            str += GetName(columns[i].ToString()) + " → " + " \" " + columns[i + 1] + " \" " + Environment.NewLine;
                        }
                    }
                    str += "磅单修改人" + " → " + " \" " + item.UpDateHistoryUser.ToString() + " \" " + Environment.NewLine;
                    str += "磅单修改时间" + " → " + " \" " + CommonHelper.Str14ToTimeFormart(item.UpDateHistoryTime.ToString()) + " \" ";
                }
            }
            MessageBox.Show(str, "修改信息");
        }

        //提示信息转化为中文
        private static string GetName(string str)
        {

            if (str == "WgtlistNo") { str = "磅单号"; }
            else if (str == "PlanNo") { str = "委托单号"; }
            else if (str == "HeatNo") { str = "炉号"; }
            else if (str == "BatchNo") { str = "炉批号"; }
            else if (str == "LronNo") { str = "铁次号"; }
            else if (str == "TankNo") { str = "铁水罐号"; }
            else if (str == "ToDeptName") { str = "去向单位"; }
            else if (str == "PlanCreateUser") { str = "委托人"; }
            else if (str == "PlanCreateTime") { str = "委托时间"; }
            else if (str == "GrossWgt") { str = "毛重(吨)"; }
            else if (str == "TareWgt") { str = "皮重(吨)"; }
            else if (str == "NetWgt") { str = "净重(吨)"; }
            else if (str == "GrossWgtTime") { str = "毛重时间"; }
            else if (str == "TareWgtTime") { str = "皮重时间"; }
            else if (str == "GrossWgtSiteName") { str = "毛重磅点"; }
            else if (str == "TareWgtSiteName") { str = "皮重磅点"; }
            else if (str == "GrossWgtMan") { str = "毛重计量员"; }
            else if (str == "TareWgtMan") { str = "皮重计量员"; }
            else if (str == "Remark") { str = "备注"; }
            else if (str == "PondRemark") { str = "磅单备注"; }
            else if (str == "CreateUser") { str = "新增人员"; }
            else if (str == "CreateTime") { str = "新增时间"; }


            //else if (str == "UpDateHistoryUser") { str = "磅单历史修改人"; }
            //else if (str == "UpDateHistoryTime") { str = "磅单历史修改时间"; }
            //else if (str == "ComputerIp") { str = "修改磅单电脑ip"; }
            //else if (str == "UpDateColumns") { str = "磅单数据更新比较"; }
            return str;
        }

    }
}