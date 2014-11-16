﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPPluginFramework
{
    public class KSPTimeSpan
    {
        //Define the Calendar
        static public int SecondsPerMinute { get; set; }
        static public int MinutesPerHour { get; set; }
        static public int HoursPerDay { get; set; }

        static public int SecondsPerHour { get { return SecondsPerMinute * MinutesPerHour; } }
        static public int SecondsPerDay { get { return SecondsPerHour * HoursPerDay; } }

        //Descriptors of Timespan - uses UT as the Root value
        public int Days
        {
            get { return (Int32)UT / SecondsPerDay; }
            set { UT = UT - Days * SecondsPerDay + value * SecondsPerDay; }
        }
        public int Hours
        {
            get { return (Int32)UT / SecondsPerHour % SecondsPerDay; }
            set { UT = UT - Hours * SecondsPerHour + value * SecondsPerHour; }
        }
        public int Minutes
        {
            get { return (Int32)UT / SecondsPerMinute % SecondsPerHour; }
            set { UT = UT - Minutes * SecondsPerMinute + value * SecondsPerMinute; }
        }
        public int Seconds
        {
            get { return (Int32)UT % SecondsPerMinute; }
            set { UT = UT - Seconds + value; }
        }
        public int Milliseconds
        {
            get { return (Int32)(Math.Round(UT - Math.Floor(UT), 3) * 1000); }
            set { UT = Math.Floor(UT) + ((Double)value / 1000); }
        }

        /// <summary>
        /// Replaces the normal "Ticks" function. This is Seconds of UT since game time 0
        /// </summary>
        public Double UT { get; set; }

        #region Constructors
        static KSPTimeSpan()
        {
            SecondsPerMinute = 60;
            MinutesPerHour = 60;
            HoursPerDay = GameSettings.KERBIN_TIME ? 6 : 24;
        }

        public KSPTimeSpan()
        {
            UT = 0;
        }
        public KSPTimeSpan(int hour, int minute, int second)
            : this()
        {
            Hours = hour; Minutes = minute; Seconds = second;
        }
        public KSPTimeSpan(int day, int hour, int minute, int second)
            : this(hour, minute,second)
        {
            Days=day;
        }
        public KSPTimeSpan(int day, int hour, int minute, int second, int millisecond)
            : this(day, hour, minute, second)
        {
            Milliseconds = millisecond;
        }

        public KSPTimeSpan(Double ut)
            : this()
        {
            UT = ut;
        } 
        #endregion


        #region Calculated Properties
        public Double TotalMilliseconds { get { return UT * 1000; } }
        public Double TotalSeconds { get { return UT; } }
        public Double TotalMinutes { get { return UT / SecondsPerMinute; } }
        public Double TotalHours { get { return UT / SecondsPerHour; } }
        public Double TotalDays { get { return UT / SecondsPerDay; } }
        #endregion

        #region Instance Methods
        #region Mathematic Methods
        public KSPTimeSpan Add(KSPTimeSpan value) {
            return new KSPTimeSpan(UT + value.UT);
        }
        public KSPTimeSpan Duration() {
            return new KSPTimeSpan(Math.Abs(UT));
        }
        public KSPTimeSpan Negate() {
            return new KSPTimeSpan(UT*-1);
        }
        #endregion

        #region Comparison Methods
        public Int32 CompareTo(KSPTimeSpan value) {
            return KSPTimeSpan.Compare(this, value);
        }
        public Int32 CompareTo(System.Object value) {
            return this.CompareTo((KSPTimeSpan)value);
        }
        public Boolean Equals(KSPTimeSpan value) {
            return KSPTimeSpan.Equals(this, value);
        }
        public override bool Equals(System.Object obj) {
            return this.Equals((KSPTimeSpan)obj);
        }
        #endregion        
        

        public override int GetHashCode()
        {
            return UT.GetHashCode();
        }

        #endregion


        #region Static Methods
        public static Int32 Compare(KSPTimeSpan t1, KSPTimeSpan t2)
        {
            if (t1.UT < t2.UT)
                return -1;
            else if (t1.UT > t2.UT)
                return 1;
            else
                return 0;
        }
        public static Boolean Equals(KSPTimeSpan t1, KSPTimeSpan t2)
        {
            return t1.UT == t2.UT;
        }


        public static KSPTimeSpan FromDays(Double value) {
            return new KSPTimeSpan(value * SecondsPerDay);
        }
        public static KSPTimeSpan FromHours(Double value) {
            return new KSPTimeSpan(value * SecondsPerHour);
        }
        public static KSPTimeSpan FromMinutes(Double value) {
            return new KSPTimeSpan(value * SecondsPerMinute);
        }
        public static KSPTimeSpan FromSeconds(Double value) {
            return new KSPTimeSpan(value);
        }
        public static KSPTimeSpan FromMilliseconds(Double value) {
            return new KSPTimeSpan(value / 1000);
        }

        #endregion

        #region Operators
        public static KSPTimeSpan operator -(KSPTimeSpan t1, KSPTimeSpan t2)
        {
            return new KSPTimeSpan(t1.UT - t2.UT);
        }
        public static KSPTimeSpan operator -(KSPTimeSpan t)
        {
            return new KSPTimeSpan(t.UT).Negate();
        }
        public static KSPTimeSpan operator +(KSPTimeSpan t1, KSPTimeSpan t2)
        {
            return new KSPTimeSpan(t1.UT + t2.UT);
        }
        public static KSPTimeSpan operator +(KSPTimeSpan t)
        {
            return new KSPTimeSpan(t.UT);
        }

        public static Boolean operator !=(KSPTimeSpan t1, KSPTimeSpan t2)
        {
            return !(t1 == t2);
        }
        public static Boolean operator ==(KSPTimeSpan t1, KSPTimeSpan t2)
        {
            return t1.UT == t2.UT;
        }



        public static Boolean operator <=(KSPTimeSpan t1, KSPTimeSpan t2)
        {
            return t1.CompareTo(t2) <= 0;
        }
        public static Boolean operator <(KSPTimeSpan t1, KSPTimeSpan t2)
        {
            return t1.CompareTo(t2) < 0;
        }
        public static Boolean operator >=(KSPTimeSpan t1, KSPTimeSpan t2)
        {
            return t1.CompareTo(t2) >= 0;
        }
        public static Boolean operator >(KSPTimeSpan t1, KSPTimeSpan t2)
        {
            return t1.CompareTo(t2) > 0;
        }
        #endregion



        //To String Formats
    }
}