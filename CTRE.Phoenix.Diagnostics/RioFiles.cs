using System;
using System.IO;

namespace CTRE.Phoenix.Diagnostics
{
    public class RioFile
    {
        protected string _sourcePath;
        protected string _sourceName;
        protected string _targetPath;

        public RioFile(string sourcePath, string targetPath)
        {
            _sourcePath = sourcePath;
            _sourceName = Path.GetFileName(_sourcePath);
            _targetPath = targetPath;
        }
        public byte[] GetContents()
        {
            try
            {
                byte[] fileContents = File.ReadAllBytes(_sourcePath);
                return fileContents;
            }
            catch (Exception)
            {

            }
            return null;
        }
        public virtual bool CheckExistanceOnPC()
        {
            return File.Exists(_sourcePath);
        }
        public virtual string TargetPath
        {
            get
            {
                return _targetPath;
            }
        }
        public virtual string BackUpPath
        {
            get
            {
                return _targetPath + ".orig";
            }
        }
        public virtual string SourcePath
        {
            get
            {
                return _sourcePath;
            }
        }
        public virtual string Sourcename
        {
            get
            {
                return _sourceName;
            }
        }
    }
    public class RioFiles
    {
        public static readonly RioFile[] kFiles = new RioFile[]
        {
            new RioFile("Binary/ctre/Phoenix-diagnostics-server", "/usr/local/frc/bin/Phoenix-diagnostics-server"),
            new RioFile("Binary/etc/init.d/Phoenix-diagnostics-server", "/etc/init.d/Phoenix-diagnostics-server"),
            new RioFile("Binary/etc/rc5.d/S25Phoenix-diagnostics-server", "/etc/rc5.d/S25Phoenix-diagnostics-server"),
            new RioFile("Binary/cci/libCTRE_PhoenixCCI.so", "/usr/local/frc/lib/libCTRE_PhoenixCCI.so"),
        };
    }
}
