/** Autogenerated file. Do not edit. */
using Jacdac;
using System;

namespace Jacdac.Clients 
{
    /// <summary>
    /// Supports aggregating timeseries data (especially sensor readings)
     /// and sending them to a cloud/storage service.
     /// Used in Jacscript.
     /// 
     /// Note that `f64` values following a label are not necessarily aligned.
    /// Implements a client for the Timeseries Aggregator service.
    /// </summary>
    /// <seealso cref="https://microsoft.github.io/jacdac-docs/services/timeseriesaggregator/" />
    public partial class TimeseriesAggregatorClient : Client
    {
        public TimeseriesAggregatorClient(JDBus bus, string name)
            : base(bus, name, ServiceClasses.TimeseriesAggregator)
        {
        }

        /// <summary>
        /// Reads the <c>now</c> register value.
        /// This register is automatically broadcast and can be also queried to establish local time on the device., _: us
        /// </summary>
        public uint Now
        {
            get
            {
                return (uint)this.GetRegisterValue((ushort)TimeseriesAggregatorReg.Now, TimeseriesAggregatorRegPack.Now);
            }
        }

        /// <summary>
        /// Reads the <c>fast_start</c> register value.
        /// When `true`, the windows will be shorter after service reset and gradually extend to requested length.
        /// This makes the sensor look more responsive., 
        /// </summary>
        public bool FastStart
        {
            get
            {
                return (bool)this.GetRegisterValueAsBool((ushort)TimeseriesAggregatorReg.FastStart, TimeseriesAggregatorRegPack.FastStart);
            }
            set
            {
                
                this.SetRegisterValue((ushort)TimeseriesAggregatorReg.FastStart, TimeseriesAggregatorRegPack.FastStart, value);
            }

        }

        /// <summary>
        /// Reads the <c>continuous_window</c> register value.
        /// Window applied to automatically created continuous timeseries.
        /// Note that windows returned initially may be shorter., _: ms
        /// </summary>
        public uint ContinuousWindow
        {
            get
            {
                return (uint)this.GetRegisterValue((ushort)TimeseriesAggregatorReg.ContinuousWindow, TimeseriesAggregatorRegPack.ContinuousWindow);
            }
            set
            {
                
                this.SetRegisterValue((ushort)TimeseriesAggregatorReg.ContinuousWindow, TimeseriesAggregatorRegPack.ContinuousWindow, value);
            }

        }

        /// <summary>
        /// Reads the <c>discrete_window</c> register value.
        /// Window applied to automatically created discrete timeseries., _: ms
        /// </summary>
        public uint DiscreteWindow
        {
            get
            {
                return (uint)this.GetRegisterValue((ushort)TimeseriesAggregatorReg.DiscreteWindow, TimeseriesAggregatorRegPack.DiscreteWindow);
            }
            set
            {
                
                this.SetRegisterValue((ushort)TimeseriesAggregatorReg.DiscreteWindow, TimeseriesAggregatorRegPack.DiscreteWindow, value);
            }

        }


        
        /// <summary>
        /// Remove all pending timeseries.
        /// </summary>
        public void Clear()
        {
            this.SendCmd((ushort)TimeseriesAggregatorCmd.Clear);
        }

        
        /// <summary>
        /// Starts a new timeseries.
        /// As for `mode`,
        /// `Continuous` has default aggregation window of 60s,
        /// and `Discrete` only stores the data if it has changed since last store,
        /// and has default window of 1s.
        /// </summary>
        public void StartTimeseries(uint id, TimeseriesAggregatorDataMode mode, string label)
        {
            this.SendCmdPacked((ushort)TimeseriesAggregatorCmd.StartTimeseries, TimeseriesAggregatorCmdPack.StartTimeseries, new object[] { id, mode, label });
        }

        
        /// <summary>
        /// Add a data point to a timeseries.
        /// </summary>
        public void Update(float value, uint id)
        {
            this.SendCmdPacked((ushort)TimeseriesAggregatorCmd.Update, TimeseriesAggregatorCmdPack.Update, new object[] { value, id });
        }

        
        /// <summary>
        /// Set aggregation window.
        /// </summary>
        public void SetWindow(uint id, uint duration)
        {
            this.SendCmdPacked((ushort)TimeseriesAggregatorCmd.SetWindow, TimeseriesAggregatorCmdPack.SetWindow, new object[] { id, duration });
        }

    }
}