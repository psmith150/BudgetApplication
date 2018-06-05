using System.Windows.Input;
using BudgetApplication.Services;
using BudgetApplication.Screens;
using Type = System.Type;

namespace BudgetApplication.Popups
{
    public class HelpViewModel : PopupViewModel
    {
        public HelpViewModel(SessionService session) : base(session)
        {
        }

        public override void Initialize(object param)
        {
            Type paramType = param.GetType();
            if(false)
            {
                this.HelpMessage = portalManagementHelpMessage;
            }
            else
            {
                this.HelpMessage = defaultHelpMessage;
            }
        }

        public override void Deinitialize()
        {
        }

        #region Public Properties
        private string _HelpMessage;
        public string HelpMessage
        {
            get
            {
                return this._HelpMessage;
            }
            set
            {
                this.Set(ref this._HelpMessage, value);
            }
        }
        #endregion

        #region Help Messages
        private const string portalManagementHelpMessage = "1. A new instance of Portal can be opened with the Open Portal button. Portal can be run either with or without the Portal GUI visible.\n\n" +
            "2. If an instance of Portal is already running, use the Connect to Portal button to select the instance and connect to it. Connecting to a new instance will close the current instance.\n\n" +
            "3. Once an instance of Portal is connected, a project needs to be loaded. An existing project can be loaded with the Open button, or a new project can be created with the New button. The currently active project will be displayed.\n\n" +
            "4. Once a project has been loaded, the active device needs to be selected. Use the gear icon to open the device manager. The current devices in the project will be displayed. An existing device can be selected, or a new one can be added with the Add Device button. A valid catalog number and firmware version are required to add a device\n\n" +
            "5. If a PLC device is active, the PLC blocks, data types, and tag tables can be displayed with the Show/Hide PLC Data button. Use the buttons to import an XML file and refresh the list, or right click on an object to export it as an XML file.\n\n" +
            "6. Once a PLC device is active, the Excel data has been loaded, and the XML templates have been loaded, use the Generate Project Data button to begin adding objects to the Portal project. You will first be prompted to choose a folder to save the XML files in. The XML files will then be created. Next, you will be prompted to locate the CPG library file. This should have been provided with this application. The library items will then be added to the PLC. Finally, the XML files will be imported to the PLC.\n\n" +
            "7. If any errors occur during this process, details will be displayed at the top of the screen. More detail can be seen in the debug window (Ctrl+Shift+D). For any assistance, please contact DMC.";
        private const string excelHelpMessage = "1. Load a PackML Excel file using the load file button.\n\n" +
            "2. Once the file is loaded, verify that the data shown is accurate. If not, correct the Excel file and reload.\n\n" +
            "3. If the file cannot be loaded, an error will be displayed at the top of the screen. More detail can be seen in the debug window (Ctrl+Shift+D). Correct these errors and reload the file.";
        private const string xmlHelpMessage = "1. Certain pre-defined XML templates must be loaded before project configuration can begin. These templates are shown in the list, and should have been included with this application. Once a template has been loaded, it will turn green and the checkbox will be checked.\n\n" +
            "2. Files can be loaded one at a time by using the Load XML File button.\n\n" +
            "3. Files can also be loaded from a folder by using the Load XML Files button. This operation will recursively load all XML files in the selected folder and all subfolders.\n\n" +
            "4. If a file cannot be loaded, an error will be displayed at the top of the screen. More detail can be seen in the debug window (Ctrl+Shift+D). Correct these errors and reload the file.";
        private const string defaultHelpMessage = "Help not defined for this screen. Contact DMC for assistance.";
        #endregion
    }
}