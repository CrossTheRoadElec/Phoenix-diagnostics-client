﻿
namespace CTRE.Phoenix.Diagnostics
{
    public enum Status
    {
        Ok = 0,

		/* Diagnostic client error codes */
        Aborted,
        BadParam,
        Busy,
        CouldNotOpenFile,
        CouldNotParseJson,
        DeviceNotFound,
        DeviceNotSelected,
        GeneralError,
        TimedOut,
        UIError,
        UnsupportedAction, //!< GUI action that has no implem
        UserCanceled,
        AsyncWebReqNotDone, //!< Sent a field upgrade web request, but did not get a resp yet.
        ServerIsFlashingAlready, //!< Attempted to field upgrade, but server is already flashing a device.

        SuccessAndConnecting, //!< Successful action, but we want to reconnect to server

        /* Error codes pertinent solely to Unit Testing */
        ProcessNotRunning,
        VersionDoesNotMatch,
        RebootTookTooLong,
        RebootTooFast,
        NotEnoughDevices,

        /* errors codes pulled from diagnostic server */
        CTRE_DI_TaskIsBusy = (-100),
        CTRE_DI_InvalidDeviceSpec = (-101),
        CTRE_DI_EcuIsNotPresent = (-102),
        CTRE_DI_CouldNotEnterBl = (-103),
        CTRE_DI_CouldNotConfirmBl = (-104),
        CTRE_DI_CouldNotErase = (-105),
        CTRE_DI_CouldNotSendFlash = (-106),
        CTRE_DI_CouldNotValidate = (-107),
        CTRE_DI_CouldNotRunApp = (-108),
        CTRE_DI_CouldNotReqSetId = (-109),
        CTRE_DI_CouldNotConfirmId = (-110),
        CTRE_DI_FlashWasGood = (-111),
        CTRE_DI_AppTooOld = (-112),
        CTRE_DI_CouldNotReqSetDesc = (-113),
        CTRE_DI_CompileSzIsWrong = (-114),
        CTRE_DI_GadgeteerDeviceNoSetId = (-115),
        CTRE_DI_InvalidTask = (-116),
        CTRE_DI_NotImplemented = (-117),
        CTRE_DI_NoDevicesOnBus = (-118),
        CTRE_DI_MoreThanOneFile = (-119),
        CTRE_DI_NodeIsInvalid = (-120),
        CTRE_DI_InvalidDeviceDescriptor = (-121),
        CTRE_DI_CouldNotSendCanFrame = (-123),
        CTRE_DI_NormalModeMsgNotPresent = (-124),
        CTRE_DI_FeatureNotSupported = (-125),
        CTRE_DI_NotUpdating = (-126),
        CTRE_DI_CorruptedPOST = (-127),
        CTRE_DI_NoConfigs = (-128),
        CTRE_DI_ConfigFailed = (-129),
        CTRE_DI_InvalidCrfBadHeader = (-200),
        CTRE_DI_InvalidCrfFileSzInvald = (-201),
        CTRE_DI_InvalidCrfWrongProduct = (-202),
        CTRE_DI_InvalidCrfNoSects = (-203),
        CTRE_DI_InvalidCrfBadSectHeader = (-204),
        CTRE_DI_InvalidCrfBadSectSize = (-205),
        CTRE_DI_NoCrfFile = (-206),
        CTRE_DI_CouldNotFindDynamicId = (-300),
        CTRE_DI_DidNotGetDhcp = (-301),
        CTRE_DI_DidNotGetFullDhcp = (-302),
        CTRE_DI_JaguarCouldNotSendSetDevId = (-400),
        CTRE_DI_JaguarBtnNotPressedApparently = (-401),
        CTRE_DI_AnotherJagALreadyHasThisId = (-402),
        CTRE_DI_InvalidJagId = (-403),
        CTRE_DI_CannotOpenSerialPort = (-500),
        CTRE_DI_CannotWriteSerialPort = (-501),
        CTRE_DI_CannotReadSerialPort = (-502),
        CTRE_DI_CannotSerialToDevice = (-503),
        CTRE_DI_NoSerialControlFrameResp = (-504),
        CTRE_DI_CannotOpenUdpPort = (-600),
        CTRE_DI_CannotWriteUdpPort = (-601),
        CTRE_DI_CannotReadUdpPort = (-602),
        CTRE_DI_CannotUdpToDevice = (-603),
        CTRE_DI_NoUdpControlFrameResp = (-604),
        CTRE_DI_TimeoutIso15Response = (-605),
    }
}
