﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KSP;
using UnityEngine;
using KSPPluginFramework;

using TWP_KACWrapper;

namespace TransferWindowPlanner
{
    //public static class OrbitExtensions
    //{
    //    //can probably be replaced with Vector3d.xzy?
    //    public static Vector3d SwapYZ(Vector3d v)
    //    {
    //        return v.Reorder(132);
    //    }

    //    //
    //    // These "Swapped" functions translate preexisting Orbit class functions into world
    //    // space. For some reason, Orbit class functions seem to use a coordinate system
    //    // in which the Y and Z coordinates are swapped.
    //    //
    //    public static Vector3d SwappedOrbitalVelocityAtUT(this Orbit o, double UT)
    //    {
    //        return SwapYZ(o.getOrbitalVelocityAtUT(UT));
    //    }

    //    //position relative to the primary
    //    public static Vector3d SwappedRelativePositionAtUT(this Orbit o, double UT)
    //    {
    //        return SwapYZ(o.getRelativePositionAtUT(UT));
    //    }

    //    //position in world space
    //    public static Vector3d SwappedAbsolutePositionAtUT(this Orbit o, double UT)
    //    {
    //        return o.referenceBody.position + o.SwappedRelativePositionAtUT(UT);
    //    }

    //}
    //public static class MathExtensions
    //{

    //    public static Vector3d Reorder(this Vector3d vector, int order)
    //    {
    //        switch (order)
    //        {
    //            case 123:
    //                return new Vector3d(vector.x, vector.y, vector.z);
    //            case 132:
    //                return new Vector3d(vector.x, vector.z, vector.y);
    //            case 213:
    //                return new Vector3d(vector.y, vector.x, vector.z);
    //            case 231:
    //                return new Vector3d(vector.y, vector.z, vector.x);
    //            case 312:
    //                return new Vector3d(vector.z, vector.x, vector.y);
    //            case 321:
    //                return new Vector3d(vector.z, vector.y, vector.x);
    //        }
    //        throw new ArgumentException("Invalid order", "order");
    //    }
    //}

#if DEBUG
    class TWPWindowDebug : MonoBehaviourWindowPlus
    {
        internal TransferWindowPlanner mbTWP;
        internal Settings settings;

        public Int32 intTest1 = 200;
        public Int32 intTest2 = 0;
        public Int32 intTest3 = 0;
        public Int32 intTest4 = 101;
        public Int32 intPlotDeparturePerDay = 1;
        public Int32 intPlotTravelPointsPerDay = 1;


        public Double dblEjectAt = 0, dblOutAngle=0;
        TransferDetails transTemp;

        double width, height;

        internal override void DrawWindow(int id)
        {
            try
            {
                DrawTextBox(ref intTest1);
                DrawTextBox(ref intTest2);
                DrawTextBox(ref intTest3);
                DrawTextBox(ref intTest4);
                DrawTextBox(ref intPlotDeparturePerDay);
                DrawTextBox(ref intPlotTravelPointsPerDay);

                Double TravelRange = (new KSPTimeSpan(mbTWP.windowMain.strTravelMaxDays, "0", "0", "0") - new KSPTimeSpan(mbTWP.windowMain.strTravelMinDays, "0", "0", "0")).UT;
                Double DepartureRange = (mbTWP.windowMain.dateMaxDeparture - mbTWP.windowMain.dateMinDeparture).UT;

                DrawLabel("Dep:{0}  ({1})", new KSPTimeSpan(DepartureRange).ToStringStandard(TimeSpanStringFormatsEnum.IntervalLong), DepartureRange);
                DrawLabel("Dep:{0}  ({1})", new KSPTimeSpan(TravelRange).ToStringStandard(TimeSpanStringFormatsEnum.IntervalLong), TravelRange);

                width = (DepartureRange / KSPDateStructure.SecondsPerDay * intPlotDeparturePerDay) + 1;
                height = (TravelRange / KSPDateStructure.SecondsPerDay * intPlotTravelPointsPerDay) + 1;

                DrawLabel("{0}x{1}", width, height);


                //DrawLabel("Default Scale: {0}",(mbTWP.windowMain.PlotWidth == 292 && mbTWP.windowMain.PlotHeight == 292));
                //if (GUILayout.Button("Reset Scale")) {
                //    mbTWP.windowMain.PlotWidth = 292;
                //    mbTWP.windowMain.PlotHeight = 292;
                //}

                //if (GUILayout.Button("Apply manual Scale (value 1 and 2)")) {
                //    mbTWP.windowMain.PlotWidth = intTest1;
                //    mbTWP.windowMain.PlotHeight = intTest2;
                //}

                if (GUILayout.Button("Apply calculates Scale and Run (value 5 - points per day)")) {
                    mbTWP.windowMain.PlotWidth = (Int32)width;
                    mbTWP.windowMain.PlotHeight = (Int32)height;

                    mbTWP.windowMain.RunPlots();
                }

                //Styles.styleTextFieldLabel.padding.top = intTest1;

                //if (GUILayout.Button("KSP")) SkinsLibrary.SetCurrent(SkinsLibrary.DefSkinType.KSP);
                //if (GUILayout.Button("UnityDef")) SkinsLibrary.SetCurrent(SkinsLibrary.DefSkinType.Unity);
                //if (GUILayout.Button("Default")) SkinsLibrary.SetCurrent("Default");
                //if (GUILayout.Button("Unity")) SkinsLibrary.SetCurrent("Unity");
                //if (GUILayout.Button("UnityWKSPButtons")) SkinsLibrary.SetCurrent("UnityWKSPButtons");

                // DrawLabel("{0}", KSPDateStructure.CalendarType);

                //if (mbTWP.btnAppLauncher != null)
                //{
                //    DrawLabel(Camera.current.WorldToScreenPoint(mbTWP.btnAppLauncher.GetAnchor()).ToString());

                //    DrawLabel(Camera.current.WorldToScreenPoint(mbTWP.btnAppLauncher.transform.localPosition).ToString());
                //}
                //if (GUILayout.Button("Make Date"))
                //{
                //    LogFormatted("a");
                //    //KSPDateTimeStructure.CalendarType = CalendarTypeEnum.Earth;
                //    KSPDateTime dt = new KSPDateTime(301.123);

                //    LogFormatted("1:{0}", dt.Minute);
                //    LogFormatted("2:{0}", dt.UT);
                //    LogFormatted("3:{0}", dt.Year);
                //    //LogFormatted("4:{0}", KSPDateTimeStructure.CalendarType);
                //    //LogFormatted("5:{0}", dt.Day);

                //}
                //if (GUILayout.Button("CreateAlarm"))
                //{
                //    String tmpID = KACWrapper.KAC.CreateAlarm(KACWrapper.KACAPI.AlarmTypeEnum.TransferModelled, 
                //        String.Format("{0} -> {1}",mbTWP.windowMain.TransferSelected.Origin.bodyName,mbTWP.windowMain.TransferSelected.Destination.bodyName), 
                //        mbTWP.windowMain.TransferSelected.DepartureTime);


                //    KACWrapper.KACAPI.KACAlarm alarmNew = KACWrapper.KAC.Alarms.First(a => a.ID == tmpID);
                //    LogFormatted("{0}==11=={1}", alarmNew.XferOriginBodyName, alarmNew.XferTargetBodyName);
                //    alarmNew.Notes = mbTWP.windowMain.GenerateTransferDetailsText();
                //    alarmNew.AlarmMargin = settings.KACMargin * 60 * 60;
                //    alarmNew.AlarmAction = settings.KACAlarmAction;
                //    LogFormatted("{0}==22=={1}", alarmNew.XferOriginBodyName, alarmNew.XferTargetBodyName);
                //    alarmNew.XferOriginBodyName = mbTWP.windowMain.TransferSelected.Origin.bodyName;
                //    LogFormatted("{0}==33=={1}", alarmNew.XferOriginBodyName, alarmNew.XferTargetBodyName);
                //    alarmNew.XferTargetBodyName = mbTWP.windowMain.TransferSelected.Destination.bodyName;

                //    LogFormatted("{0}======{1}", alarmNew.XferOriginBodyName, alarmNew.XferTargetBodyName);
                    
                //}
                //DrawLabel("Windowpadding:{0}", SkinsLibrary.CurrentSkin.window.padding);

                //DrawLabel("{0}", KACWrapper.KAC.Alarms.Count);
                //foreach ( KACWrapper.KACAPI.KACAlarm a in KACWrapper.KAC.Alarms)
                //{
                //    DrawLabel("{1}-{2} ({0})", a.ID,a.Name, a.AlarmType);
                //}


                //DrawLabel("MouseOverAny:{0}", mbTWP.MouseOverAnyWindow);
                //DrawLabel("MouseOverMain:{0}", mbTWP.windowMain.IsMouseOver);
                //DrawLabel("MouseOverMain:{0}", mbTWP.windowMain.IsMouseOver);

                //DrawLabel("Settings:{0}", mbTWP.windowSettings.WindowRect);
                //DrawLabel("SettingsBlocker:{0}", mbTWP.windowSettingsBlockout.WindowRect);
                //DrawLabel("CI:{0} P:{1}", mbTWP.windowMain.ColorIndex, mbTWP.windowMain.Percent);

                DrawLabel("Mouse:{0}", mbTWP.windowMain.vectMouse);
                DrawLabel("Plot:{0}", new Rect(mbTWP.windowMain.PlotPosition.x, mbTWP.windowMain.PlotPosition.y, mbTWP.windowMain.PlotWidth, mbTWP.windowMain.PlotHeight));
                DrawLabel("Selected:{0}", mbTWP.windowMain.vectSelected);
                DrawLabel("Departure:{0:0}, Travel:{1:0}", mbTWP.windowMain.DepartureSelected / KSPDateStructure.SecondsPerDay, mbTWP.windowMain.TravelSelected / KSPDateStructure.SecondsPerDay);

                //if (mbTWP.windowMain.TransferSelected != null && FlightGlobals.ActiveVessel!=null)
                //{
                //    if (GUILayout.Button("FindUT")) {
                //        dblEjectAt = Utilities.timeOfEjectionAngle(FlightGlobals.ActiveVessel.orbit, mbTWP.windowMain.TransferSelected.DepartureTime, mbTWP.windowMain.TransferSelected.EjectionAngle * LambertSolver.Rad2Deg, 20, out dblOutAngle);
                //        intTest5 = (Int32)dblEjectAt;
                //    }
                //    DrawLabel("UT: {0:0}", dblEjectAt);
                //    DrawLabel("Angle: {0:0.000}", dblOutAngle);

                //    DrawLabel("UTSelect: {0:0}", mbTWP.windowMain.TransferSelected.DepartureTime);
                //    DrawLabel("OrbitPeriod: {0:0}", FlightGlobals.ActiveVessel.orbit.period);
                //    DrawLabel("Scan: {0:0}->{1:0}", mbTWP.windowMain.TransferSelected.DepartureTime - FlightGlobals.ActiveVessel.orbit.period / 2, mbTWP.windowMain.TransferSelected.DepartureTime + FlightGlobals.ActiveVessel.orbit.period / 2);


                //    if (GUILayout.Button("NewTransfer"))
                //    {
                //        transTemp = new TransferDetails();
                //        LambertSolver.TransferDeltaV(mbTWP.windowMain.TransferSelected.Origin, mbTWP.windowMain.TransferSelected.Destination,
                //            intTest5, mbTWP.windowMain.TransferSelected.TravelTime, FlightGlobals.ActiveVessel.orbit.getRelativePositionAtUT(intTest5).magnitude - FlightGlobals.ActiveVessel.orbit.referenceBody.Radius, mbTWP.windowMain.TransferSpecs.FinalOrbitAltitude, out transTemp);
                //        transTemp.CalcEjectionValues();
                //    }

                //    DrawLabel("v1:{0:0.000}  edv:{1:0.000}", LambertSolver.v1out, LambertSolver.vedvout);

                //    if (transTemp != null)
                //    {
                //        GUILayout.Label(transTemp.TransferDetailsText);
                //        if (GUILayout.Button("Newnode"))
                //        {
                //            FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Clear();
                //            ManeuverNode mNode = FlightGlobals.ActiveVessel.patchedConicSolver.AddManeuverNode(transTemp.DepartureTime);
                //            mNode.DeltaV = new Vector3d(0, transTemp.EjectionDVNormal, transTemp.EjectionDVPrograde);
                //            FlightGlobals.ActiveVessel.patchedConicSolver.UpdateFlightPlan();

                //        }

                //    }

                //}


                //DrawLabel("Ass:{0}", KACWrapper.AssemblyExists);
                //DrawLabel("Ins:{0}", KACWrapper.InstanceExists);
                //DrawLabel("API:{0}", KACWrapper.APIReady);

                //if (mbTWP.windowMain.TransferSelected != null)
                //{
                //    DrawLabel("D:{0:0} T:{0:0}", mbTWP.windowMain.TransferSelected.DepartureTime, mbTWP.windowMain.TransferSelected.TravelTime);
                //    DrawLabel("Origin:{0}", mbTWP.windowMain.TransferSelected.OriginVelocity);
                //    DrawLabel("Initial:{0}", mbTWP.windowMain.TransferSelected.TransferInitalVelocity);
                //    DrawLabel("Final:{0}", mbTWP.windowMain.TransferSelected.TransferFinalVelocity);
                //    DrawLabel("Destination:{0}", mbTWP.windowMain.TransferSelected.DestinationVelocity);
                //    DrawLabel("Eject:{0}", mbTWP.windowMain.TransferSelected.EjectionDeltaVector);
                //    DrawLabel("Eject-Mag:{0}", mbTWP.windowMain.TransferSelected.EjectionDeltaVector.magnitude);
                //    DrawLabel("Insert:{0}", mbTWP.windowMain.TransferSelected.InjectionDeltaVector);
                //    DrawLabel("Insert-Mag:{0}", mbTWP.windowMain.TransferSelected.InjectionDeltaVector.magnitude);
                //    DrawLabel("TransferAngle:{0}", mbTWP.windowMain.TransferSelected.TransferAngle);
                //    DrawLabel("EjectionInclination:{0}", mbTWP.windowMain.TransferSelected.EjectionInclination);
                //    DrawLabel("InsertionInclination:{0}", mbTWP.windowMain.TransferSelected.InsertionInclination);

                //    DrawLabel("Phase:{0}", mbTWP.windowMain.TransferSelected.PhaseAngle);

                //    DrawLabel("EjectionDVNormal:{0}", mbTWP.windowMain.TransferSelected.EjectionDVNormal);
                //    DrawLabel("EjectionDVPrograde:{0}", mbTWP.windowMain.TransferSelected.EjectionDVPrograde);
                //    DrawLabel("EjectionHeading:{0}", mbTWP.windowMain.TransferSelected.EjectionHeading);
                //    DrawLabel("EjectionVector:{0}", mbTWP.windowMain.TransferSelected.EjectionVector);
                //    DrawLabel("EjectionAngle:{0}", mbTWP.windowMain.TransferSelected.EjectionAngle);

                //}

                //DrawLabel("Padding:{0}", SkinsLibrary.CurrentSkin.window.padding);
                //DrawLabel("Margin:{0}", SkinsLibrary.CurrentSkin.window.margin);
                //DrawLabel("Border:{0}", SkinsLibrary.CurrentSkin.window.border);

                //DrawLabel("Padding:{0}", SkinsLibrary.DefKSPSkin.window.padding);
                //DrawLabel("Margin:{0}", SkinsLibrary.DefKSPSkin.window.margin);
                //DrawLabel("Border:{0}", SkinsLibrary.DefKSPSkin.window.border);

                //CelestialBody cbK = FlightGlobals.Bodies[0];
                //CelestialBody cbO = FlightGlobals.Bodies.FirstOrDefault(x => x.bodyName.ToLower() == "kerbin");
                //CelestialBody cbD = FlightGlobals.Bodies.FirstOrDefault(x => x.bodyName.ToLower() == "duna");

                ////This is the frame of Reference rotation that is occuring
                //QuaternionD Rot = Planetarium.ZupRotation;
                //DrawLabel("Rotation:{0}", Rot);

                //DrawLabel("Kerbin");
                //DrawLabel("True Anomaly:{0}", cbO.orbit.trueAnomaly);
                //DrawLabel("True Anomaly at 0:{0}", cbO.orbit.TrueAnomalyAtUT(0));
                //DrawLabel("True Anomaly at first:{0}", cbO.orbit.TrueAnomalyAtUT(intTest1));
                //DrawLabel("Velocity at first:{0}", cbO.orbit.getOrbitalVelocityAtUT(intTest1).magnitude);
                //DrawLabel("Velocity at first:{0}", cbO.orbit.getOrbitalVelocityAtUT(intTest1));
                //DrawLabel("Pos at firstT:{0}", cbO.orbit.getRelativePositionAtUT(intTest1).magnitude);

                ////We have to remove the frame of ref rotation to get static values to plot
                //DrawLabel("Pos at firstT:{0}", Quaternion.Inverse(Rot) * cbO.orbit.getRelativePositionAtUT(intTest1));
                ////DrawLabel("Pos at firstUT:{0}", cbO.orbit.getRelativePositionAtT(intTest1));

                //DrawLabel("AbsPos at firstUT:{0}", cbO.orbit.getPositionAtUT(intTest1));

                //DrawLabel("Duna");
                //DrawLabel("True Anomaly:{0}", cbD.orbit.trueAnomaly);
                //DrawLabel("True Anomaly at 0:{0}", cbD.orbit.TrueAnomalyAtUT(0));
                //DrawLabel("True Anomaly at first:{0}", cbD.orbit.TrueAnomalyAtUT(intTest1));
                //DrawLabel("Velocity at first:{0}", cbD.orbit.getOrbitalVelocityAtUT(intTest1).magnitude);
                //DrawLabel("Velocity at first:{0}", cbD.orbit.getOrbitalVelocityAtUT(intTest1));
                //DrawLabel("Pos at firstT:{0}", cbD.orbit.getRelativePositionAtUT(intTest1).magnitude);

                ////We have to remove the frame of ref rotation to get static values to plot
                //DrawLabel("Pos at firstT:{0}", Quaternion.Inverse(Rot) * cbD.orbit.getRelativePositionAtUT(intTest1));
                ////DrawLabel("Pos at firstUT:{0}", cbD.orbit.getRelativePositionAtT(intTest1));

                //DrawLabel("AbsPos at firstUT:{0}", cbD.orbit.getPositionAtUT(intTest1));


                //DrawLabel("DepartureMin:{0}", mbTWP.windowMain.dateMinDeparture.UT);
                //DrawLabel("DepartureRange:{0}", DepartureRange);
                //DrawLabel("DepartureMax:{0}", mbTWP.windowMain.dateMaxDeparture.UT);
                //DrawLabel("TravelMin:{0}", TravelMin);
                //DrawLabel("TravelMax:{0}", TravelMax);

                //DrawLabel("Hohmann:{0}", hohmannTransferTime);
                //DrawLabel("synodic:{0}", synodicPeriod);

                //DrawLabel("Ktime:{0}", kTime.DateString());

                //if (GUILayout.Button("A number!"))
                //{
                //    CelestialBody cbO = FlightGlobals.Bodies.FirstOrDefault(x => x.bodyName.ToLower() == "kerbin");
                //    CelestialBody cbD = FlightGlobals.Bodies.FirstOrDefault(x => x.bodyName.ToLower() == "duna");
                //    LogFormatted_DebugOnly("Next UT:{0}->{1}: {2}",cbO.bodyName,cbD.bodyName,LambertSolver.NextLaunchWindowUT(cbO,cbD));
                //}

                //if (GUILayout.Button("A DV!"))
                //{
                //    CelestialBody cbO = FlightGlobals.Bodies.FirstOrDefault(x => x.bodyName.ToLower() == "kerbin");
                //    CelestialBody cbD = FlightGlobals.Bodies.FirstOrDefault(x => x.bodyName.ToLower() == "duna");
                //    LogFormatted_DebugOnly("DV:{0}->{1}: {2}", cbO.bodyName, cbD.bodyName, LambertSolver.TransferDeltaV(cbO, cbD, 5030208, 5718672, 100000, 100000));
                //}

                //if (GUILayout.Button("Solve!"))
                //{
                //    CelestialBody cbO = FlightGlobals.Bodies.FirstOrDefault(x => x.bodyName.ToLower() == "kerbin");
                //    CelestialBody cbD = FlightGlobals.Bodies.FirstOrDefault(x => x.bodyName.ToLower() == "duna");
                //    Vector3d originPositionAtDeparture = cbO.orbit.getRelativePositionAtUT(5030208);
                //    Vector3d destinationPositionAtArrival = cbD.orbit.getRelativePositionAtUT(5030208 + 5718672);
                //    bool longWay = Vector3d.Cross(originPositionAtDeparture, destinationPositionAtArrival).y < 0;

                //    LogFormatted_DebugOnly("DV:{0}->{1}: {2}", cbO.bodyName, cbD.bodyName, LambertSolver.Solve(cbO.referenceBody.gravParameter, originPositionAtDeparture, destinationPositionAtArrival, 5718672, longWay));
                //    LogFormatted_DebugOnly("Origin:{0}", originPositionAtDeparture);
                //    LogFormatted_DebugOnly("Dest:{0}", destinationPositionAtArrival);
                //}

                //if (GUILayout.Button("Maths"))
                //{
                //    CelestialBody cbK = FlightGlobals.Bodies[0];
                //    CelestialBody cbO = FlightGlobals.Bodies.FirstOrDefault(x => x.bodyName.ToLower() == "kerbin");
                //    CelestialBody cbD = FlightGlobals.Bodies.FirstOrDefault(x => x.bodyName.ToLower() == "duna");

                //    LogFormatted_DebugOnly("TAat0:{0}: {1}", cbO.bodyName, cbO.orbit.TrueAnomalyAtUT(0));
                //    LogFormatted_DebugOnly("TAat0:{0}: {1}", cbD.bodyName, cbD.orbit.TrueAnomalyAtUT(0));
                //    LogFormatted_DebugOnly("TAat5030208:{0}: {1}", cbO.bodyName, cbO.orbit.TrueAnomalyAtUT(5030208));
                //    LogFormatted_DebugOnly("TAat5030208:{0}: {1}", cbD.bodyName, cbD.orbit.TrueAnomalyAtUT(5030208));
                //    LogFormatted_DebugOnly("OVat5030208:{0}: {1}", cbO.bodyName, cbO.orbit.getOrbitalVelocityAtUT(5030208).magnitude);
                //    LogFormatted_DebugOnly("OVat5030208:{0}: {1}", cbD.bodyName, cbD.orbit.getOrbitalVelocityAtUT(5030208).magnitude);
                //    LogFormatted_DebugOnly("RPat5030208:{0}: X:{1},Y:{2},Z:{3}", cbO.bodyName, cbO.orbit.getRelativePositionAtUT(5030208).x, cbO.orbit.getRelativePositionAtUT(5030208).y, cbO.orbit.getRelativePositionAtUT(5030208).z);
                //    LogFormatted_DebugOnly("RPat5030208:{0}: X:{1},Y:{2},Z:{3}", cbD.bodyName, cbD.orbit.getRelativePositionAtUT(5030208).x, cbD.orbit.getRelativePositionAtUT(5030208).y, cbD.orbit.getRelativePositionAtUT(5030208).z);

                //    LogFormatted_DebugOnly("RPat5030208:{0}: X:{1},Y:{2},Z:{3}", cbO.bodyName, cbO.orbit.getRelativePositionAtUT(5030208 - Planetarium.GetUniversalTime()).x, cbO.orbit.getRelativePositionAtUT(5030208 - Planetarium.GetUniversalTime()).y, cbO.orbit.getRelativePositionAtUT(5030208 - Planetarium.GetUniversalTime()).z);
                //    LogFormatted_DebugOnly("RPat5030208:{0}: X:{1},Y:{2},Z:{3}", cbD.bodyName, cbD.orbit.getRelativePositionAtUT(5030208 - Planetarium.GetUniversalTime()).x, cbD.orbit.getRelativePositionAtUT(5030208 - Planetarium.GetUniversalTime()).y, cbD.orbit.getRelativePositionAtUT(5030208 - Planetarium.GetUniversalTime()).z);

                //    //LogFormatted_DebugOnly("SwapRPat5030208:{0}: X:{1},Y:{2},Z:{3}", cbO.bodyName, cbO.orbit.SwappedRelativePositionAtUT(5030208).x, cbO.orbit.SwappedRelativePositionAtUT(5030208).y, cbO.orbit.SwappedRelativePositionAtUT(5030208).z);
                //    //LogFormatted_DebugOnly("SwapRPat5030208:{0}: X:{1},Y:{2},Z:{3}", cbD.bodyName, cbD.orbit.SwappedRelativePositionAtUT(5030208).x, cbD.orbit.SwappedRelativePositionAtUT(5030208).y, cbD.orbit.SwappedRelativePositionAtUT(5030208).z);

                //    ////LogFormatted_DebugOnly("Absat5030208:{0}: {1}", cbK.bodyName, getAbsolutePositionAtUT(cbK.orbit, 5030208));
                //    //LogFormatted_DebugOnly("Absat5030208:{0}: {1}", cbO.bodyName, getAbsolutePositionAtUT(cbO.orbit,5030208));
                //    //LogFormatted_DebugOnly("Absat5030208:{0}: {1}", cbD.bodyName, getAbsolutePositionAtUT(cbD.orbit, 5030208));

                //    //LogFormatted_DebugOnly("Posat5030208:{0}: {1}", cbO.bodyName, cbO.getPositionAtUT(5030208));
                //    //LogFormatted_DebugOnly("TPosat5030208:{0}: {1}", cbO.bodyName, cbO.getTruePositionAtUT(5030208));
                //    //LogFormatted_DebugOnly("Posat5030208:{0}: {1}", cbD.bodyName, cbD.getPositionAtUT(5030208));
                //    //LogFormatted_DebugOnly("TPosat5030208:{0}: {1}", cbD.bodyName, cbD.getTruePositionAtUT(5030208));
                //    //LogFormatted_DebugOnly("Posat5030208:{0}: {1}", cbK.bodyName, cbK.getPositionAtUT(5030208));
                //    //LogFormatted_DebugOnly("TPosat5030208:{0}: {1}", cbK.bodyName, cbK.getTruePositionAtUT(5030208));



                //    Vector3d pos1 = new Vector3d(13028470326,3900591743,0);
                //    Vector3d pos2 = new Vector3d(-19970745720,-1082561296,15466922.92);
                //    double tof = 5718672;

                //    Vector3d vdest;
                //    Vector3d vinit = LambertSolver.Solve(cbK.gravParameter, pos1, pos2, tof, false, out vdest);
                //    LogFormatted_DebugOnly("Init:{0} - {1}", vinit.magnitude, vinit);
                //    LogFormatted_DebugOnly("vdest:{0} - {1}", vdest.magnitude, vdest);


                //    Vector3d vr1 = cbO.orbit.getOrbitalVelocityAtUT(5030208);
                //    Vector3d vr2 = cbD.orbit.getOrbitalVelocityAtUT(5030208 + 5718672);

                //    Vector3d vdest2;
                //    Vector3d vinit2;
                //    LambertSolver2.Solve(pos1, pos2, tof,cbK, true,out vinit2, out vdest2);

                //    LogFormatted_DebugOnly("Origin:{0} - {1}", vr1.magnitude, vr1);
                //    LogFormatted_DebugOnly("Dest:{0} - {1}", vr2.magnitude, vr2);

                //    LogFormatted_DebugOnly("Depart:{0} - {1}", vinit2.magnitude, vinit2);
                //    LogFormatted_DebugOnly("Arrive:{0} - {1}", vdest2.magnitude, vdest2);

                //    LogFormatted_DebugOnly("Eject:{0} - {1}", (vinit2 - vr1).magnitude, vinit2 - vr1);
                //    LogFormatted_DebugOnly("Inject:{0} - {1}", (vdest2 - vr2).magnitude, vdest2 - vr2);

                //}

                
            }
            catch (Exception)
            {

            }

        }


    }
#endif
}
