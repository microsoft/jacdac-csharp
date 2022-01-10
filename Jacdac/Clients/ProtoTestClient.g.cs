/** Autogenerated file. Do not edit. */
using Jacdac;
using System;

namespace Jacdac.Clients {

    /// <summary>
    /// This is test service to validate the protocol packet transmissions between the browser and a MCU.
     /// Use this page if you are porting Jacdac to a new platform.
    /// Implements a client for the Protocol Test service.
    /// </summary>
    /// <seealso cref="https://microsoft.github.io/jacdac-docs/services/prototest/" />
    public partial class ProtoTestClient : Client
    {
        public ProtoTestClient(JDBus bus, string name)
            : base(bus, name, ServiceClasses.ProtoTest)
        {
        }

        /// <summary>
        /// Reads the <c>rw_bool</c> register value.
        /// A read write bool register., 
        /// </summary>
        public bool RwBool
        {
            get
            {
                return (bool)this.GetRegisterValue((ushort)ProtoTestReg.RwBool, ProtoTestRegPack.RwBool);
            }
            set
            {
                
                this.SetRegisterValue((ushort)ProtoTestReg.RwBool, ProtoTestRegPack.RwBool, value);
            }

        }

        /// <summary>
        /// Reads the <c>ro_bool</c> register value.
        /// A read only bool register. Mirrors rw_bool., 
        /// </summary>
        public bool RoBool
        {
            get
            {
                return (bool)this.GetRegisterValue((ushort)ProtoTestReg.RoBool, ProtoTestRegPack.RoBool);
            }
        }

        /// <summary>
        /// Reads the <c>rw_u32</c> register value.
        /// A read write u32 register., 
        /// </summary>
        public uint RwU32
        {
            get
            {
                return (uint)this.GetRegisterValue((ushort)ProtoTestReg.RwU32, ProtoTestRegPack.RwU32);
            }
            set
            {
                
                this.SetRegisterValue((ushort)ProtoTestReg.RwU32, ProtoTestRegPack.RwU32, value);
            }

        }

        /// <summary>
        /// Reads the <c>ro_u32</c> register value.
        /// A read only u32 register.. Mirrors rw_u32., 
        /// </summary>
        public uint RoU32
        {
            get
            {
                return (uint)this.GetRegisterValue((ushort)ProtoTestReg.RoU32, ProtoTestRegPack.RoU32);
            }
        }

        /// <summary>
        /// Reads the <c>rw_i32</c> register value.
        /// A read write i32 register., 
        /// </summary>
        public int RwI32
        {
            get
            {
                return (int)this.GetRegisterValue((ushort)ProtoTestReg.RwI32, ProtoTestRegPack.RwI32);
            }
            set
            {
                
                this.SetRegisterValue((ushort)ProtoTestReg.RwI32, ProtoTestRegPack.RwI32, value);
            }

        }

        /// <summary>
        /// Reads the <c>ro_i32</c> register value.
        /// A read only i32 register.. Mirrors rw_i32., 
        /// </summary>
        public int RoI32
        {
            get
            {
                return (int)this.GetRegisterValue((ushort)ProtoTestReg.RoI32, ProtoTestRegPack.RoI32);
            }
        }

        /// <summary>
        /// Reads the <c>rw_string</c> register value.
        /// A read write string register., 
        /// </summary>
        public string RwString
        {
            get
            {
                return (string)this.GetRegisterValue((ushort)ProtoTestReg.RwString, ProtoTestRegPack.RwString);
            }
            set
            {
                
                this.SetRegisterValue((ushort)ProtoTestReg.RwString, ProtoTestRegPack.RwString, value);
            }

        }

        /// <summary>
        /// Reads the <c>ro_string</c> register value.
        /// A read only string register. Mirrors rw_string., 
        /// </summary>
        public string RoString
        {
            get
            {
                return (string)this.GetRegisterValue((ushort)ProtoTestReg.RoString, ProtoTestRegPack.RoString);
            }
        }

        /// <summary>
        /// Reads the <c>rw_bytes</c> register value.
        /// A read write string register., 
        /// </summary>
        public byte[] RwBytes
        {
            get
            {
                return (byte[])this.GetRegisterValue((ushort)ProtoTestReg.RwBytes, ProtoTestRegPack.RwBytes);
            }
            set
            {
                
                this.SetRegisterValue((ushort)ProtoTestReg.RwBytes, ProtoTestRegPack.RwBytes, value);
            }

        }

        /// <summary>
        /// Reads the <c>ro_bytes</c> register value.
        /// A read only string register. Mirrors ro_bytes., 
        /// </summary>
        public byte[] RoBytes
        {
            get
            {
                return (byte[])this.GetRegisterValue((ushort)ProtoTestReg.RoBytes, ProtoTestRegPack.RoBytes);
            }
        }

        /// <summary>
        /// Reads the <c>rw_i8_u8_u16_i32</c> register value.
        /// A read write i8, u8, u16, i32 register., 
        /// </summary>
        public object[] /*(int, uint, uint, int)*/ RwI8U8U16I32
        {
            get
            {
                return (object[] /*(int, uint, uint, int)*/)this.GetRegisterValues((ushort)ProtoTestReg.RwI8U8U16I32, ProtoTestRegPack.RwI8U8U16I32);
            }
            set
            {
                
                this.SetRegisterValues((ushort)ProtoTestReg.RwI8U8U16I32, ProtoTestRegPack.RwI8U8U16I32, value);
            }

        }

        /// <summary>
        /// Reads the <c>ro_i8_u8_u16_i32</c> register value.
        /// A read only i8, u8, u16, i32 register.. Mirrors rw_i8_u8_u16_i32., 
        /// </summary>
        public object[] /*(int, uint, uint, int)*/ RoI8U8U16I32
        {
            get
            {
                return (object[] /*(int, uint, uint, int)*/)this.GetRegisterValues((ushort)ProtoTestReg.RoI8U8U16I32, ProtoTestRegPack.RoI8U8U16I32);
            }
        }

        /// <summary>
        /// Reads the <c>rw_u8_string</c> register value.
        /// A read write u8, string register., 
        /// </summary>
        public object[] /*(uint, string)*/ RwU8String
        {
            get
            {
                return (object[] /*(uint, string)*/)this.GetRegisterValues((ushort)ProtoTestReg.RwU8String, ProtoTestRegPack.RwU8String);
            }
            set
            {
                
                this.SetRegisterValues((ushort)ProtoTestReg.RwU8String, ProtoTestRegPack.RwU8String, value);
            }

        }

        /// <summary>
        /// Reads the <c>ro_u8_string</c> register value.
        /// A read only u8, string register.. Mirrors rw_u8_string., 
        /// </summary>
        public object[] /*(uint, string)*/ RoU8String
        {
            get
            {
                return (object[] /*(uint, string)*/)this.GetRegisterValues((ushort)ProtoTestReg.RoU8String, ProtoTestRegPack.RoU8String);
            }
        }

        /// <summary>
        /// An event raised when rw_bool is modified
        /// </summary>
        public event ClientEventHandler EBool
        {
            add
            {
                this.AddEvent((ushort)ProtoTestEvent.EBool, value);
            }
            remove
            {
                this.RemoveEvent((ushort)ProtoTestEvent.EBool, value);
            }
        }

        /// <summary>
        /// An event raised when rw_u32 is modified
        /// </summary>
        public event ClientEventHandler EU32
        {
            add
            {
                this.AddEvent((ushort)ProtoTestEvent.EU32, value);
            }
            remove
            {
                this.RemoveEvent((ushort)ProtoTestEvent.EU32, value);
            }
        }

        /// <summary>
        /// An event raised when rw_i32 is modified
        /// </summary>
        public event ClientEventHandler EI32
        {
            add
            {
                this.AddEvent((ushort)ProtoTestEvent.EI32, value);
            }
            remove
            {
                this.RemoveEvent((ushort)ProtoTestEvent.EI32, value);
            }
        }

        /// <summary>
        /// An event raised when rw_string is modified
        /// </summary>
        public event ClientEventHandler EString
        {
            add
            {
                this.AddEvent((ushort)ProtoTestEvent.EString, value);
            }
            remove
            {
                this.RemoveEvent((ushort)ProtoTestEvent.EString, value);
            }
        }

        /// <summary>
        /// An event raised when rw_bytes is modified
        /// </summary>
        public event ClientEventHandler EBytes
        {
            add
            {
                this.AddEvent((ushort)ProtoTestEvent.EBytes, value);
            }
            remove
            {
                this.RemoveEvent((ushort)ProtoTestEvent.EBytes, value);
            }
        }

        /// <summary>
        /// An event raised when rw_i8_u8_u16_i32 is modified
        /// </summary>
        public event ClientEventHandler EI8U8U16I32
        {
            add
            {
                this.AddEvent((ushort)ProtoTestEvent.EI8U8U16I32, value);
            }
            remove
            {
                this.RemoveEvent((ushort)ProtoTestEvent.EI8U8U16I32, value);
            }
        }

        /// <summary>
        /// An event raised when rw_u8_string is modified
        /// </summary>
        public event ClientEventHandler EU8String
        {
            add
            {
                this.AddEvent((ushort)ProtoTestEvent.EU8String, value);
            }
            remove
            {
                this.RemoveEvent((ushort)ProtoTestEvent.EU8String, value);
            }
        }


        
        /// <summary>
        /// A command to set rw_bool.
        /// </summary>
        public void CBool(bool bo)
        {
            this.SendCmdPacked((ushort)ProtoTestCmd.CBool, ProtoTestCmdPack.CBool, new object[] { bo });
        }

        
        /// <summary>
        /// A command to set rw_u32.
        /// </summary>
        public void CU32(uint u32)
        {
            this.SendCmdPacked((ushort)ProtoTestCmd.CU32, ProtoTestCmdPack.CU32, new object[] { u32 });
        }

        
        /// <summary>
        /// A command to set rw_i32.
        /// </summary>
        public void CI32(int i32)
        {
            this.SendCmdPacked((ushort)ProtoTestCmd.CI32, ProtoTestCmdPack.CI32, new object[] { i32 });
        }

        
        /// <summary>
        /// A command to set rw_string.
        /// </summary>
        public void CString(string str)
        {
            this.SendCmdPacked((ushort)ProtoTestCmd.CString, ProtoTestCmdPack.CString, new object[] { str });
        }

        
        /// <summary>
        /// A command to set rw_string.
        /// </summary>
        public void CBytes(byte[] bytes)
        {
            this.SendCmdPacked((ushort)ProtoTestCmd.CBytes, ProtoTestCmdPack.CBytes, new object[] { bytes });
        }

        
        /// <summary>
        /// A command to set rw_bytes.
        /// </summary>
        public void CI8U8U16I32(int i8, uint u8, uint u16, int i32)
        {
            this.SendCmdPacked((ushort)ProtoTestCmd.CI8U8U16I32, ProtoTestCmdPack.CI8U8U16I32, new object[] { i8, u8, u16, i32 });
        }

        
        /// <summary>
        /// A command to set rw_u8_string.
        /// </summary>
        public void CU8String(uint u8, string str)
        {
            this.SendCmdPacked((ushort)ProtoTestCmd.CU8String, ProtoTestCmdPack.CU8String, new object[] { u8, str });
        }

    }
}