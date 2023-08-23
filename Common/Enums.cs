using System;

namespace FTN.Common
{	
	public enum PhaseCode : short
	{
		Unknown = 0x0,
		N = 0x1,
		C = 0x2,
		CN = 0x3,
		B = 0x4,
		BN = 0x5,
		BC = 0x6,
		BCN = 0x7,
		A = 0x8,
		AN = 0x9,
		AC = 0xA,
		ACN = 0xB,
		AB = 0xC,
		ABN = 0xD,
		ABC = 0xE,
		ABCN = 0xF
	}
	
	public enum TransformerFunction : short
	{
		Supply = 1,				// Supply transformer
		Consumer = 2,			// Transformer supplying a consumer
		Grounding = 3,			// Transformer used only for grounding of network neutral
		Voltreg = 4,			// Feeder voltage regulator
		Step = 5,				// Step
		Generator = 6,			// Step-up transformer next to a generator.
		Transmission = 7,		// HV/HV transformer within transmission network.
		Interconnection = 8		// HV/HV transformer linking transmission network with other transmission networks.
	}
	
	public enum WindingConnection : short
	{
		Y = 1,		// Wye
		D = 2,		// Delta
		Z = 3,		// ZigZag
		I = 4,		// Single-phase connection. Phase-to-phase or phase-to-ground is determined by elements' phase attribute.
		Scott = 5,   // Scott T-connection. The primary winding is 2-phase, split in 8.66:1 ratio
		OY = 6,		// 2-phase open wye. Not used in Network Model, only as result of Topology Analysis.
		OD = 7		// 2-phase open delta. Not used in Network Model, only as result of Topology Analysis.
	}

	public enum WindingType : short
	{
		None = 0,
		Primary = 1,
		Secondary = 2,
		Tertiary = 3
	}

    public enum UnitSymbol : short
    {

        /// Current in ampere.
        A=1,
        /// Plane angle in degrees.
        deg=2,
        /// Relative temperature in degrees Celsius. In the SI unit system the symbol is ºC. Electric charge is measured in coulomb that has the unit symbol C. To destinguish degree Celsius form coulomb the symbol used in the UML is degC. Reason for not using ºC is the special character º is difficult to manage in software.
        degC=3,
        /// Capacitance in farad.
        F=4,
        /// Mass in gram.
        g=5,
        /// Time in hours.
        h=6,
        /// Inductance in henry.
        H=7,
        /// Frequency in hertz.
        Hz=8,
        /// Energy in joule.
        J=9,
        /// Length in meter.
        m=10,
        /// Area in square meters.
        m2=11,
        /// Volume in cubic meters.
        m3=12,
        /// Time in minutes.
        min=13,
        /// Force in newton.
        N=14,
        /// Dimension less quantity, e.g. count, per unit, etc.
        none=15,
        /// Resistance in ohm.
        ohm=16,
        /// Pressure in pascal (n/m2).
        Pa=17,
        /// Plane angle in radians.
        rad=18,
        /// Time in seconds.
        s=19,
        /// Conductance in siemens.
        S=20,
        /// Voltage in volt.
        V=21,
        /// Apparent power in volt ampere.
        VA=22,
        /// Apparent energy in volt ampere hours.
        VAh=23,
        /// Reactive power in volt ampere reactive.
        VAr=24,
        /// Reactive energy in volt ampere reactive hours.
        VArh=25,
        /// Active power in watt.
        W=26,
        /// Real energy in what hours.
        Wh=27,
    }

    public enum UnitMultiplier : short
    {

        /// Centi 10**-2.
        c=1,
        /// Deci 10**-1.
        d=2,
        /// Giga 10**9.
        G=3,
        /// Kilo 10**3.
        k=4,
        /// Milli 10**-3.
        m=5,
        /// Mega 10**6.
        M=6,
        /// Micro 10**-6.
        micro=7,
        /// Nano 10**-9.
        n=8,
        /// No multiplier or equivalently multiply by 1.
        none=9,
        /// Pico 10**-12.
        p=10,
        /// Tera 10**12.
        T=11,
    }

    public enum SwitchState : short
    {

        /// Switch is closed.
        close=0,

        /// Switch is open.
        open=1,
    }
}
