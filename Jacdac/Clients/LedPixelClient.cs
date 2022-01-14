﻿using System;

namespace Jacdac.Clients
{
    public partial class LedPixelClient
    {
        /// <summary>
        /// Runs an encoded light command
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        public void Run(string source, object[] args)
        {
            var payload = LedPixelProgramEncoding.ToBuffer(source, args);
            this.SendCmdPacked((ushort)LedPixelCmd.Run, LedPixelCmdPack.Run, new object[] { payload });
        }
    }
}
