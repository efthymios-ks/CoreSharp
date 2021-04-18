using System;
using System.Collections.Generic;

namespace CoreSharp.Sources
{
    internal static class AsciiControls
    {
        //Fields 
        internal static char NUL => Convert.ToChar(0);
        internal static char SOH => Convert.ToChar(1);
        internal static char STX => Convert.ToChar(2);
        internal static char ETX => Convert.ToChar(3);
        internal static char EOT => Convert.ToChar(4);
        internal static char ENQ => Convert.ToChar(5);
        internal static char ACK => Convert.ToChar(6);
        internal static char BEL => Convert.ToChar(7);
        internal static char BS => Convert.ToChar(8);
        internal static char TAB => Convert.ToChar(9);
        internal static char LF => Convert.ToChar(10);
        internal static char VT => Convert.ToChar(11);
        internal static char FF => Convert.ToChar(12);
        internal static char CR => Convert.ToChar(13);
        internal static char SO => Convert.ToChar(14);
        internal static char SI => Convert.ToChar(15);
        internal static char DLE => Convert.ToChar(16);
        internal static char DC1 => Convert.ToChar(17);
        internal static char DC2 => Convert.ToChar(18);
        internal static char DC3 => Convert.ToChar(19);
        internal static char DC4 => Convert.ToChar(20);
        internal static char NAK => Convert.ToChar(21);
        internal static char SYN => Convert.ToChar(22);
        internal static char ETB => Convert.ToChar(23);
        internal static char CAN => Convert.ToChar(24);
        internal static char EM => Convert.ToChar(25);
        internal static char SUB => Convert.ToChar(26);
        internal static char ESC => Convert.ToChar(27);
        internal static char FS => Convert.ToChar(28);
        internal static char GS => Convert.ToChar(29);
        internal static char RS => Convert.ToChar(30);
        internal static char US => Convert.ToChar(31);

        /// <summary>
        /// List with ASCII control characters and their abbreviations. 
        /// </summary>
        internal static readonly IDictionary<char, string> Dictionary = new Dictionary<char, string>()
        {
            { NUL, nameof(NUL) },
            { SOH, nameof(SOH) },
            { STX, nameof(STX) },
            { ETX, nameof(ETX) },
            { EOT, nameof(EOT) },
            { ENQ, nameof(ENQ) },
            { ACK, nameof(ACK) },
            { BEL, nameof(BEL) },
            { BS, nameof(BS) },
            { TAB, nameof(TAB) },
            { LF, nameof(LF) },
            { VT, nameof(VT) },
            { FF, nameof(FF) },
            { CR, nameof(CR) },
            { SO, nameof(SO) },
            { SI, nameof(SI) },
            { DLE, nameof(DLE) },
            { DC1, nameof(DC1) },
            { DC2, nameof(DC2) },
            { DC3, nameof(DC3) },
            { DC4, nameof(DC4) },
            { NAK, nameof(NAK) },
            { SYN, nameof(SYN) },
            { ETB, nameof(ETB) },
            { CAN, nameof(CAN) },
            { EM, nameof(EM) },
            { SUB, nameof(SUB) },
            { ESC, nameof(ESC) },
            { FS, nameof(FS) },
            { GS, nameof(GS) },
            { RS, nameof(RS) },
            { US, nameof(US) }
        };
    }
}
