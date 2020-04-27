using System;
using System.Collections.Generic;
using libKimai.models;
using MySql.Data.MySqlClient;

namespace libKimai.query
{
    public class Activities : Base
    {
        public DateTime? FromDate { get; set; }

        private TimeZoneInfo _timeZone { get; set; }
        private string _sqlStatementBase = @"select SQL_NO_CACHE kt.id as 'Id', ka.name as 'ActivityName', ka.comment as 'ActivityComment',
            kp.name as 'ProjectName', kp.comment as 'ProjectComment', kc.name as 'Customer',
            kc.hourly_rate as 'HourlyRate', kt.start_time as 'StartTime', kt.end_time as 'EndTime',
            kt.description as 'TimeNotes' from kimai2_timesheet kt
	        left join kimai2_activities ka on (kt.activity_id = ka.id)
            left join kimai2_projects kp on (ka.project_id = kp.id)
            left join kimai2_customers kc on (kp.customer_id = kc.id)";

        public Activities(string queryString, string timeZone = "Central Standard Time") : base(queryString)
        {
            _timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
        }

        public List<Activity> GetActivities(bool orderByDesc = false)
        {
            string sqlStatement = BuildStatement(orderByDesc);
            
            MySqlConnection connection = new MySqlConnection(_mysqlConnectionString);
            connection.Open();
            
            MySqlCommand cmd = new MySqlCommand(sqlStatement, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            List<Activity> activities = new List<Activity>();
            while (reader.Read())
            {
                activities.Add(CreateActivity(reader));
            }

            connection.Close();
            connection.Dispose();
            return activities;
        }

        public string BuildStatement(bool orderByDesc = false)
        {
            string sqlStatement = _sqlStatementBase;

            if (FromDate != null)
            {
                sqlStatement += $" where start_time > '{FromDate.Value.ToString("yyyy/MM/dd")}' ";
            }
            
            if (orderByDesc)
            {
                sqlStatement += " order by start_time desc";
            }

            return sqlStatement;
        }
        
        public Activity CreateActivity(MySqlDataReader record)
        {
            Activity activity = new Activity()
            {
                Id = long.Parse(record.GetValue(record.GetOrdinal("Id")).ToString()),
                ActivityName = record.GetValue(record.GetOrdinal("ActivityName")) as string,
                ActivityComment = record.GetValue(record.GetOrdinal("ActivityComment")) as string,
                ProjectName = record.GetValue(record.GetOrdinal("ProjectName")) as string,
                ProjectComment = record.GetValue(record.GetOrdinal("ProjectComment")) as string,
                Customer = record.GetValue(record.GetOrdinal("Customer")) as string,
                TimeNotes = record.GetValue(record.GetOrdinal("TimeNotes")) as string
            };
	
            Object value = record.GetValue(record.GetOrdinal("HourlyRate"));
	
            if(!(value is System.DBNull)) {
                activity.HourlyRate = (double) value;
            }

            value = record.GetValue(record.GetOrdinal("StartTime"));
            if (!(value is System.DBNull))
            {
                DateTime utcTime = (DateTime) value;
                activity.StartTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, _timeZone);
            }

            value = record.GetValue(record.GetOrdinal("EndTime"));
            if (!(value is System.DBNull))
            {
                DateTime utcTime = (DateTime)value;
                activity.EndTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, _timeZone);
            }

            return activity;
        }
    }
}