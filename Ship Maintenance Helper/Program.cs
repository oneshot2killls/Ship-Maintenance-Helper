using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        StringBuilder _echoBuilder = new StringBuilder(512);//just in case echos get crazy, use this for organization

        //Interior Turret Helper
        bool _interiorTurretHelperEnabled = false;
        bool _interiorTurretHelperShouldAutoUpdate = false;
        InteriorTurretHelper _interiorTurretHelper = new InteriorTurretHelper();
        List<IMyLargeInteriorTurret> _interiorTurretList = new List<IMyLargeInteriorTurret>();

        //Turret Unfucker    
        bool _turretUnfuckerEnabled = false;
        TurretUnfucker _turretUnfucker = new TurretUnfucker();
        List<IMyLargeTurretBase> _turretUnfuckerTurretList = new List<IMyLargeTurretBase>();

        //Turret Reset
        bool _turretPositionResetEnabled = false;
        TurretPositionReset _turretPositionReset = new TurretPositionReset();
        List<IMyLargeTurretBase> _turretPositionResetTurretList = new List<IMyLargeTurretBase>();


        //Ini
        MyIni _ini = new MyIni();
        StringBuilder _customDataSB = new StringBuilder();


        const string IniSectionConfig = "SMH Config";
        const string IniConfigTurretUnfucker = "Turret Unfucker";
        const string IniConfigTurretReset = "Turret Position Reset";
        const string IniConfigTurretHelper = "Interior Turret Helper";

        const string IniConfigTurretHelperAutoUpdate = "Interior Turret Helper Auto Update";

        public Program()
        {
            ParseIni();
            //Read ini and get blocks
            if (_interiorTurretHelperEnabled)
            {
                GridTerminalSystem.GetBlocksOfType<IMyLargeInteriorTurret>(_interiorTurretList);
                _interiorTurretHelper.CreateInteriorTurretHelper(_interiorTurretList);
                Echo("- - - - - - - - - -\nInterior Turret Helper:\n");
                Echo($"Interior turrets: {_interiorTurretHelper.m_interiorTurretList.Count}");
            }
            if (_turretUnfuckerEnabled)
            {
                GridTerminalSystem.GetBlocksOfType<IMyLargeInteriorTurret>(_interiorTurretList);
                _turretUnfucker.CreateTurretUnfucker(_turretUnfuckerTurretList);
                Echo("- - - - - - - - - -\nTurret Unfucker:\n");
                Echo($"Turrets: {_turretUnfucker.m_turretList.Count}");
            }
            if (_turretPositionResetEnabled)
            {
                GridTerminalSystem.GetBlocksOfType<IMyLargeTurretBase>(_turretPositionResetTurretList);
                _turretPositionReset.CreateTurretPositionReset(_turretPositionResetTurretList);
                Echo("- - - - - - - - - -\nTurret Position Reset:\n");
                Echo($"Turrets: {_turretPositionReset.m_turretList.Count}");

            }
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }

        public void Save()
        {
            
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (_interiorTurretHelperEnabled)
            {
                if (_interiorTurretHelper.m_interiorTurretList.Count == 0)
                {
                    GridTerminalSystem.GetBlocksOfType<IMyLargeInteriorTurret>(_interiorTurretList);
                    _interiorTurretHelper.CreateInteriorTurretHelper(_interiorTurretList);
                    Echo("- - - - - - - - - -\nInterior Turret Helper:\n");
                    Echo($"Interior turrets: {_interiorTurretHelper.m_interiorTurretList.Count}");                
                }
                else
                {
                    Echo("- - - - - - - - - -\nInterior Turret Helper:\n");
                    Echo($"Interior turrets: {_interiorTurretHelper.m_interiorTurretList.Count}");
                    string parsedArgument;
                    if (ArgumentContains("ITH:",argument, out parsedArgument))
                    {
                        _interiorTurretHelper.Main(parsedArgument);
                    }
                    if (_interiorTurretHelperShouldAutoUpdate)
                    {
                        _interiorTurretHelper.Main("run");
                    }
                    Echo(_interiorTurretHelper.m_echoBuilder.ToString());
                }
            }
            if (_turretUnfuckerEnabled)
            {
                if (_turretUnfucker.m_turretList.Count == 0)
                {
                    GridTerminalSystem.GetBlocksOfType<IMyLargeTurretBase>(_turretUnfuckerTurretList);
                    _turretUnfucker.CreateTurretUnfucker(_turretUnfuckerTurretList);
                    Echo("- - - - - - - - - -\nTurret Unfucker:\n");
                    Echo($"Turrets: {_turretUnfucker.m_turretList.Count}");
                }
                else
                {
                    Echo("- - - - - - - - - -\nTurret Unfucker:\n");
                    Echo($"Turrets: {_turretUnfucker.m_turretList.Count}");
                    string parsedArgument;
                    if (ArgumentContains("TU:", argument, out parsedArgument))
                    {
                        _turretUnfucker.Main(parsedArgument);
                    }                  
                    Echo(_turretUnfucker.m_echoBuilder.ToString());
                }
            }
            if (_turretPositionResetEnabled)
            {
                if(_turretPositionReset.m_turretList.Count == 0)
                {
                    GridTerminalSystem.GetBlocksOfType<IMyLargeTurretBase>(_turretPositionResetTurretList);
                    _turretPositionReset.CreateTurretPositionReset(_turretPositionResetTurretList);
                    Echo("- - - - - - - - - -\nTurret Position Reset:\n");
                    Echo($"Turrets: {_turretPositionReset.m_turretList.Count}");
                }
                else
                {
                    Echo("- - - - - - - - - -\nTurret Position Reset:\n");
                    Echo($"Turrets: {_turretPositionReset.m_turretList.Count}");
                    string parsedArgument;
                    if (ArgumentContains("TPR:", argument, out parsedArgument))
                    {
                        _turretPositionReset.Main(parsedArgument);
                    }
                    Echo(_turretPositionReset.m_echoBuilder.ToString());
                }
            }

            //just in case echos get crazy, use _echoBuilder for organization
            string output = _echoBuilder.ToString();
            base.Echo(output);
            _echoBuilder.Clear();
        }

        public bool ArgumentContains(string prefix, string input, out string output)
        {           
            if (prefix.Length > input.Length)
            {
                output = "";
                return true;
            }
            if (StringContains(input.Substring(0,prefix.Length).Trim(),prefix))
            {      
                output = input.Substring(prefix.Length, input.Length - prefix.Length);
                //output = input.Substring(prefix.Length, input.Length - prefix.Length);
                return true;
            }
            output = "";
            return false;
        }

        public static bool StringContains(string source, string toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public void ParseIni()
        {
            _ini.Clear();
            string customData = Me.CustomData;
            bool parsed = _ini.TryParse(customData);

            if (!parsed && !string.IsNullOrWhiteSpace(Me.CustomData.Trim()))
            {
                _ini.EndContent = Me.CustomData;
            }

            List<string> sections = new List<string>();
            _ini.GetSections(sections);

            if (sections.Count == 0)
            {
                _customDataSB.Clear();
                _customDataSB.Append(Me.CustomData);
                _customDataSB.Replace("---\n", "");

                _ini.EndContent = _customDataSB.ToString();
            }

            //Config
            //Example.... _ini.Get(Section Name, variable name).ToWhatEver(Default Value of Variable if can't parse.. just use the variable's current value)
            _interiorTurretHelperEnabled = _ini.Get(IniSectionConfig, IniConfigTurretHelper).ToBoolean(_interiorTurretHelperEnabled);
            _interiorTurretHelperShouldAutoUpdate = _ini.Get(IniSectionConfig, IniConfigTurretHelperAutoUpdate).ToBoolean(_interiorTurretHelperShouldAutoUpdate);
            _turretUnfuckerEnabled = _ini.Get(IniSectionConfig, IniConfigTurretUnfucker).ToBoolean(_turretUnfuckerEnabled);
            _turretPositionResetEnabled = _ini.Get(IniSectionConfig, IniConfigTurretReset).ToBoolean(_turretPositionResetEnabled);

            WriteIni();
        }

        public void WriteIni()
        {
            //Example... _ini.Set(Section Name, Variable name, value)
            _ini.Set(IniSectionConfig, IniConfigTurretHelper, _interiorTurretHelperEnabled);
            _ini.Set(IniSectionConfig, IniConfigTurretHelperAutoUpdate, _interiorTurretHelperShouldAutoUpdate);
            _ini.Set(IniSectionConfig, IniConfigTurretUnfucker, _turretUnfuckerEnabled);
            _ini.Set(IniSectionConfig, IniConfigTurretReset, _turretPositionResetEnabled);


            string output = _ini.ToString();
            if (!string.Equals(output, Me.CustomData))
            {
                Me.CustomData = output;
            }
        }

        public class InteriorTurretHelper
        {
            public string m_argumentGlobal = "";
            public int m_ammoThreshold = 10;
            private int m_lowAmmoTurrets = 0;
            public List<IMyLargeInteriorTurret> m_interiorTurretList = new List<IMyLargeInteriorTurret>();

            public StringBuilder m_echoBuilder = new StringBuilder();
            public void CreateInteriorTurretHelper(List<IMyLargeInteriorTurret> interiorTurrets)
            {
                m_interiorTurretList = interiorTurrets;
            }
            public void Main(string argument)
            {
                if (m_argumentGlobal.Equals("") || !m_argumentGlobal.Equals(argument))
                {
                    m_argumentGlobal = argument;
                }
                switch (m_argumentGlobal.ToLower())
                {
                    case "":
                        //m_echoBuilder.Clear();
                        //m_echoBuilder.Append($"Ammo Threshold: {m_ammoThreshold}\n");
                        //m_echoBuilder.Append($"Low Ammo Interior Turrets: {m_lowAmmoTurrets}\n");
                        break;
                    case "run":
                        m_echoBuilder.Clear();
                        m_echoBuilder.Append($"Ammo Threshold: {m_ammoThreshold}\n");
                        m_echoBuilder.Append($"Low Ammo Interior Turrets: {m_lowAmmoTurrets}\n");
                        break;
                    case "reset":
                        Reset();
                        break;
                    default:
                        try
                        {
                            m_ammoThreshold = Int32.Parse(m_argumentGlobal);
                            m_echoBuilder.Clear();
                            m_echoBuilder.Append($"Ammo Threshold: {m_ammoThreshold}\n");
                            m_echoBuilder.Append($"Low Ammo Interior Turrets: {m_lowAmmoTurrets}\n");
                            //Echo($"Ammo Threshold: {m_ammoThreshold}\n");
                        }
                        catch (FormatException e)
                        {
                            m_echoBuilder.Append($"Invalid argument! Valid arguments are: \"reset\" or an Integer\nArgument: {argument}");
                        }
                        break;
                }
                ToggleShowOnHud();
            }

            private void ToggleShowOnHud()
            {
                m_lowAmmoTurrets = 0;
                foreach (IMyLargeInteriorTurret InteriorTurret in m_interiorTurretList)
                {
                    int magazineCount = 0;
                    if (InteriorTurret.GetInventory().GetItemAt(0) == null)
                    {
                        InteriorTurret.ShowOnHUD = true;
                        m_lowAmmoTurrets++;
                        continue;
                    }
                    IMyInventory myInventory = InteriorTurret.GetInventory();
                    List<MyInventoryItem> myInventoryItems = new List<MyInventoryItem>();
                    InteriorTurret.GetInventory().GetItems(myInventoryItems);
                    magazineCount = (int)myInventoryItems[0].Amount;
                    //Echo($"magazineCount: {InteriorTurret.DisplayNameText}");
                    //Echo($"magazineCount: {magazineCount}\n");

                    if (magazineCount < m_ammoThreshold)
                    {
                        InteriorTurret.ShowOnHUD = true;
                        m_lowAmmoTurrets++;              
                    }
                    else
                    {
                        InteriorTurret.ShowOnHUD = false;
                    }
                }               
            }

            public void Reset()
            {
                m_interiorTurretList = new List<IMyLargeInteriorTurret>();
                m_ammoThreshold = 10;
                m_argumentGlobal = "";
            }
        }

        public class TurretUnfucker
        {
            public string m_argumentGlobal = "";
            private int m_turretsUnfucked = 0;

            public List<IMyLargeTurretBase> m_turretList = new List<IMyLargeTurretBase>();
            public StringBuilder m_echoBuilder = new StringBuilder();

            public void CreateTurretUnfucker(List<IMyLargeTurretBase> turrets)
            {
                m_turretList = turrets;
            }
            public void Main(string argument)
            {
                if (m_argumentGlobal.Equals("") || !m_argumentGlobal.Equals(argument))
                {
                    m_argumentGlobal = argument;
                }
                switch (m_argumentGlobal.ToLower())
                {
                    case "":
                        break;
                    case "run":
                        UnfuckTurrets();
                        m_echoBuilder.Clear();
                        m_echoBuilder.Append($"Turrets Unfucked: {m_turretsUnfucked}\n");
                        break;
                    case "reset":
                        Reset();
                        m_echoBuilder.Clear();
                        m_echoBuilder.Append($"Turrets Unfucked: {m_turretsUnfucked}\n");
                        break;
                    default:
                        m_echoBuilder.Append($"Invalid argument! Valid arguments are: \"run\" or \"reset\"\nArgument: {argument}");
                        break;
                }
            }

            public void UnfuckTurrets()
            {
                m_turretsUnfucked = 0;
                foreach (IMyLargeTurretBase turret in m_turretList)
                {

                    //store turret values
                    bool enabled = turret.Enabled;
                    bool targetMeteors = turret.TargetMeteors;
                    bool targetMissiles = turret.TargetMissiles;
                    bool targetCharacters = turret.TargetCharacters;
                    bool targetSmallGrids = turret.TargetSmallGrids;
                    bool targetLargeGrids = turret.TargetLargeGrids;
                    bool targetStations = turret.TargetStations;
                    float range = turret.Range;
                    bool enableIdleRotation = turret.EnableIdleRotation;

                    turret.ResetTargetingToDefault();

                    //restore turret values
                    turret.Enabled = enabled;
                    turret.TargetMeteors = targetMeteors;
                    turret.TargetMissiles = targetMissiles;
                    turret.TargetCharacters = targetCharacters;
                    turret.TargetSmallGrids = targetSmallGrids;
                    turret.TargetLargeGrids = targetLargeGrids;
                    turret.TargetStations = targetStations;
                    turret.Range = range;
                    turret.EnableIdleRotation = enableIdleRotation;

                    m_turretsUnfucked++;
                }
            }
            public void Reset()
            {
                m_turretList = new List<IMyLargeTurretBase>();
                m_turretsUnfucked = 0;
                m_argumentGlobal = "";
            }
        }

        public class TurretPositionReset
        {
            public List<IMyLargeTurretBase> m_turretList = new List<IMyLargeTurretBase>();
            public StringBuilder m_echoBuilder = new StringBuilder();
            public string m_argumentGlobal = "";
            private int m_turretsReset = 0;

            public void CreateTurretPositionReset(List<IMyLargeTurretBase> turrets)
            {
                m_turretList = turrets;
            }
            public void Main(string argument)
            {
                if (m_argumentGlobal.Equals("") || !m_argumentGlobal.Equals(argument))
                {
                    m_argumentGlobal = argument;
                }
                switch (m_argumentGlobal.ToLower())
                {
                    case "":                       
                        break;
                    case "run":
                        ResetPosition();
                        m_echoBuilder.Clear();
                        m_echoBuilder.Append($"Turrets Reset: {m_turretsReset}\n");
                        break;
                    case "reset":
                        Reset();
                        m_echoBuilder.Clear();
                        m_echoBuilder.Append($"Turrets Reset: {m_turretsReset}\n");
                        break;
                    default:
                        m_echoBuilder.Append($"Invalid argument! Valid arguments are: \"run\" or \"reset\"\nArgument: {argument}");
                        break;
                }
            }
            public void ResetPosition()
            {
                m_turretsReset = 0;
                for (int i = 0; i < m_turretList.Count; i++)
                {
                    m_turretList[i].Elevation = 0;
                    m_turretList[i].Azimuth = 0;
                    m_turretList[i].ResetTargetingToDefault();
                    m_turretList[i].EnableIdleRotation = false;

                    m_turretsReset++;
                }

            }
            public void Reset()
            {
                m_turretList = new List<IMyLargeTurretBase>();
                m_turretsReset = 0;
                m_argumentGlobal = "";
            }
        }

    }
}
