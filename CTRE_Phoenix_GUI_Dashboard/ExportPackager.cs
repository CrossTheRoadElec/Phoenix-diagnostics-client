using CTRE.Phoenix.Diagnostics.BackEnd;
using CTRE.Phoenix.Diagnostics;
using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace CTRE_Phoenix_GUI_Dashboard
{
    public class ExportPackager
    {
        DataGridView gridDiagnosticLog;
        DeviceListContainer _deviceListContainer;
        ListView lstDevices;
        ComboBox cboHostSelectorAddr;
        ComboBox cboHostSelectorPrt;

        public ExportPackager(DataGridView gridDiagnosticLog,
                                DeviceListContainer _deviceListContainer,
                                ListView lstDevices,
                                ComboBox cboHostSelectorAddr,
                                ComboBox cboHostSelectorPrt)
        {
            this.gridDiagnosticLog = gridDiagnosticLog;
            this._deviceListContainer = _deviceListContainer;
            this.lstDevices = lstDevices;
            this.cboHostSelectorAddr = cboHostSelectorAddr;
            this.cboHostSelectorPrt = cboHostSelectorPrt;
        }
        public Status Export()
        {
            try
            {
                /* If directory doesn't exist, create it */
                if (!Directory.Exists(".\\Exports"))
                {
                    Directory.CreateDirectory(".\\Exports");
                }

                /* Create directory for this specific Export */
                string newExportPath = string.Format(".\\Exports\\Export {0}", DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss"));
                Directory.CreateDirectory(newExportPath);

                using (FileStream diagLog = new FileStream(newExportPath + "\\Diagnostic Log.txt", FileMode.Create))
                {
                    /* Export data to a file */
                    ExportDiagnosticLog(diagLog);

                }
                using (FileStream devLog = new FileStream(newExportPath + "\\Devices Log.txt", FileMode.Create))
                {
                    /* Export data to a file */
                    ExportDevices(devLog);
                }
                using (FileStream info = new FileStream(newExportPath + "\\Client Info.txt", FileMode.Create))
                {
                    /* Export data to a file */
                    ExportClientInfo(info);
                }
                /* Zip up all files */
                ZipEverythingUp(newExportPath, newExportPath + ".zip");

                /* Delete original directory to save space */
                new DirectoryInfo(newExportPath).Delete(true);

                System.Diagnostics.Process.Start(".\\Exports");

                /* Action Completed, report success */
                return Status.Ok;
            }
            catch (FileNotFoundException){
                /*  Export Folder/File could not be found, return code (Security/Permission settings) */
                return Status.CouldNotFindExportFolder;
            };
        }

        private void ExportDiagnosticLog(FileStream diagLog)
        {
            string content = "";
            foreach (DataGridViewRow row in gridDiagnosticLog.Rows)
            {
                /* Format data in tab-delimited txt file */
                content += row.Cells[0].Value + "\t" +
                    row.Cells[1].Value + "\t" +
                    row.Cells[2].Value + "\r\n";
            }
            byte[] contentBytes = System.Text.Encoding.UTF8.GetBytes(content);
            diagLog.Write(contentBytes, 0, contentBytes.Length);
        }

        private void ExportDevices(FileStream devLog)
        {
            string content = "";
            foreach (ListViewItem item in lstDevices.Items)
            {
                CTRE.Phoenix.Diagnostics.DeviceDescrip device;
                _deviceListContainer.GetDescriptor(item, out device);
                content += device.jsonStrings.Name + "\t" +
                    device.jsonStrings.SoftStatus + "\t" +
                    device.jsonStrings.Model + "\t" +
                    device.jsonStrings.ID + "\t" +
                    device.jsonStrings.CurrentVers + "\t" +
                    device.jsonStrings.ManDate + "\t" +
                    device.jsonStrings.BootloaderRev + "\t" +
                    device.jsonStrings.HardwareRev + "\r\n";
            }
            byte[] contentBytes = System.Text.Encoding.UTF8.GetBytes(content);
            devLog.Write(contentBytes, 0, contentBytes.Length);
        }

        private void ExportClientInfo(FileStream connectionInfo)
        {
            string a, b, c;
            BackEnd.Instance.GetStatus(out a, out b, out c);

            string content = "";
            content += "IP Address is: " + cboHostSelectorAddr.Text + "\r\n";
            content += "Port is: " + cboHostSelectorPrt.Text + "\r\n";
            content += "Backend Status is: " + a + "\r\n";
            content += "Server Version is: " + BackEnd.Instance.GetVersionNumbers() + "\r\n";
            content += "Client Version is: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\r\n";

            byte[] contentBytes = System.Text.Encoding.UTF8.GetBytes(content);
            connectionInfo.Write(contentBytes, 0, contentBytes.Length);
        }

        private  void ZipEverythingUp(string sourceDestination, string zipDestination)
        {
            ZipFile.CreateFromDirectory(sourceDestination, zipDestination);
        }
    }
}
