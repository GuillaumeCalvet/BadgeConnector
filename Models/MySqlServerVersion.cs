﻿using System;

namespace LedConnector.Models
{
    internal class MySqlServerVersion
    {
        private Version version;

        public MySqlServerVersion(Version version)
        {
            this.version = version;
        }
    }
}