using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stay_calm_db_tests
{
    public class hgstatus_db
    {
        [Key]
        public int id { get; set; }

        public int fourb_ch_main_pump { get; set; }

        public int fourb_ch_addition_pump { get; set; }

        public int fourb_byte_acid_base_pump { get; set; }

        public int sixb_low_reagent { get; set; }

        public int sevenb_flow_meter { get; set; }

        public int eightb_incorrect_pass { get; set; }

        public int eightb_parameter_out_of_range { get; set; }

        public int eightb_amount_parameters_over_max { get; set; }

        public int elevenb_M3_H_GPM { get; set; }

        public int elevenb_total_ch_onoff { get; set; }

        public int elevenb_Celsius_F { get; set; }

        public int elevenb_ch_alarm_onoff { get; set; }

        public int elevenb_ch_avg_onoff { get; set; }

        public int elevenb_turbidity_onoff { get; set; }

        public int elevenb_flow_sensor_onoff { get; set; }

        public int elevenb_byte_alkali_acid { get; set; }

        public int twelveb_temp_onoff { get; set; }

        public int twelveb_input_onoff { get; set; }

        public int twelveb_flow_onoff { get; set; }

        public int twelveb_conductivity_onoff { get; set; }

        public int twelveb_ph_onoff { get; set; }

        public int twelveb_ORP_onoff { get; set; }

        public int twelveb_free_ch_onoff { get; set; }

        public int thirteenb_low_chlor { get; set; }

        public int thirteenb_replace_light { get; set; }

        public int thirteenb_unclean_cell { get; set; }

        public int thirteenb_ORP { get; set; }

        public int thirteenb_ch_below_zeropointone { get; set; }

        public int thirteenb_no_reagents { get; set; }

        public int thirteenb_low_flow { get; set; }

        public int thirteenb_no_flow { get; set; }

        public int fourteenb_Colorimetr_comm_error { get; set; }

        public int fourteenb_external_off { get; set; }

        public int fourteenb_High_NTU { get; set; }

        public int fourteenb_Low_ORP { get; set; }

        public int fourteenb_High_Ph { get; set; }

        public int fourteenb_Low_Ph { get; set; }

        public int fourteenb_High_chlor { get; set; }

        public int fifteenb_High_temperature { get; set; }

        public int fifteenb_Low_temperature { get; set; }

        public int fifteenb_Piston_stuck { get; set; }

        public int fifteenb_Ph_overfeed_time { get; set; }

        public int fifteenb_Chlor_overfeed_time { get; set; }

        public int fifteenb_No_DPD3 { get; set; }

        public int fifteenb_High_combine_chlorine { get; set; }

        public int fifteenb_High_total_chlor { get; set; }

        public int sixteen_Low_Cassette { get; set; }

        public int sixteen_Cassette { get; set; }

        public int sixteen_End_of_Cassette { get; set; }

        public int sixteen_Door { get; set; }

        public int sixteen_pH_Tank_Empty { get; set; }

        public int sixteen_CL_Tank_Empty { get; set; }

        public int sixteen_Calibration_Fail { get; set; }

        public DateTime date_record { get; set; }

        [ForeignKey("siteuser")]
        public int serial_site_id { get; set; }

        public virtual siteuser siteuser { get; set; }
    }
}