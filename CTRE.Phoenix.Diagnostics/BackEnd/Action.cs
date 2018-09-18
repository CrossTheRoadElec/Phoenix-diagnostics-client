namespace CTRE.Phoenix.Diagnostics.BackEnd
{
    /// <summary>
    /// Data storage for Front End's action request.
    /// </summary>
    public class Action
    {
        // ------------ Types --------- //
        /// <summary>
        /// Callback to application (typically the GUI)
        /// </summary>
        /// <param name="action"> action that was executed </param>
        /// <param name="err"> Success/Failure error code </param>
        public delegate void CallBack(Action action, Status err);

        // ------------ Minimum stuff for a typical action, which device and what to do --------- //

        public ActionType type; //!< What operation to take

        /// <summary>
        /// The model type.
        /// </summary>
        public string model;

        /// <summary>
        /// Ecu address of device to operate on.
        /// </summary>
        public byte deviceID;

        /// <summary>
        /// Application's callback when action complete.
        /// </summary>
        public CallBack callback;

        // ------------ Misc additional info action may require --------- //

        /// <summary>
        /// Only filled if action is Self-Test.
        /// </summary>
        public string selfTestResults;


        /// <summary>
        /// If Action requires access to a file, save the path.
        /// Typically this will be the CRF path for field-upgrades.
        /// </summary>
        public string filePath;
        

        ///// <summary>
        ///// For set-ID actions, stores the new request ID
        ///// </summary>
        public uint newID;


        /// <summary>
        /// Error status of this action.
        /// </summary>
        public Status Error
        {
            get; internal set;
        }

        /// <summary>
        /// General string input, some commmands take a single string input, 
        /// like setting the device name.
        /// </summary>
        public string stringParam;

        public uint param;
        // ------------ C'tors ------------ //
        public Action()
        {

        }
        public Action(CallBack callback, string model, byte deviceID, ActionType type)
        {
            this.callback = callback;
            this.model = model;
            this.deviceID = deviceID;
            this.type = type;
        }
        public Action(CallBack callback, DeviceDescrip dd, ActionType type)
        {
            this.callback = callback;
            this.model = dd.model;
            this.deviceID = dd.deviceID;
            this.type = type;
        }
        /* Only to be used for actions that do not involve any devices */
        public Action(CallBack callback, ActionType type)
        {
            this.callback = callback;
            this.type = type;
        }
    }
}
