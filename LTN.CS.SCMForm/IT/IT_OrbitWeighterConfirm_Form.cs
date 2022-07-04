﻿using LTN.CS.Base;
using LTN.CS.SCMForm.Common;
using LTN.CS.SCMService.IT.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LTN.CS.SCMForm.IT
{
    public partial class IT_OrbitWeighterConfirm_Form : CoreForm
    {
        public IIT_OrbitWeighterConfirmService Mainservice { get; set; }
        public IT_OrbitWeighterConfirm_Form()
        {
            InitializeComponent();
        }
       
        /// <summary>
        /// 查询方法
        /// </summary>
        private void Query()
        {
            Hashtable condition = new Hashtable();
            if (!string.IsNullOrEmpty(date_StartTime.Text))
            {
                condition.Add("StartTime", CommonHelper.TimeToStr14(Convert.ToDateTime(date_StartTime.Text)));
            }
            if (!string.IsNullOrEmpty(date_EndTime.Text))
            {
                condition.Add("EndTime", CommonHelper.TimeToStr14(Convert.ToDateTime(date_EndTime.Text)));
            }
            if (!string.IsNullOrEmpty(calibrateNo.Text))
            {
                condition.Add("Calibrate_no", calibrateNo.Text);
            }
            gcl_main.DataSource = Mainservice.ExecuteDB_QueryByCalibrateAndTime(condition);
            gvw_main.BestFitColumns();

        }
        private void IT_OrbitWeighterConfirm_Form_Shown(object sender, EventArgs e)
        {
            date_StartTime.Text = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd 00:00:00");
            date_EndTime.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
            Query();
        }

        private void gvw_main_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "T_CALIBRATE_TIME")
            {
                if (e.Value != null)
                {
                    e.DisplayText = CommonHelper.Str14ToTimeFormart(e.Value.ToString());
                }
            }
            /*
            if (e.Column.Name == "gCol_State_Id")
            {
                if (e.Value != null)
                {
                    switch (e.Value.ToString().Trim())
                    {
                        case "0":
                            e.DisplayText = "自动";
                            break;
                        case "1":
                            e.DisplayText = "手动";
                            break;
                        default:
                            e.DisplayText = "";
                            break;
                    }
                }
            }
             * */
            if (e.Column.Name == "T_UPLOAD_FLAG")
            {
                if (e.Value != null)
                {
                    switch (e.Value.ToString().Trim())
                    {
                        case "N":
                            e.DisplayText = "未接受";
                            break;
                        case "Y":
                            e.DisplayText = "接受";
                            break;
                        default:
                            e.DisplayText = "";
                            break;
                    }
                }
            }

        }

        private void gvw_main_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

        }

        private void btn_Query_Click(object sender, EventArgs e)
        {
            Query();
        }
    }
}
