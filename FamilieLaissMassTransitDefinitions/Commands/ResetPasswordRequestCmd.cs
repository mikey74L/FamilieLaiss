using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamilieLaissMassTransitDefinitions.Commands
{
    /// <summary>
    /// Reset password request (Command-Class for MassTransit)
    /// </summary>
    public class ResetPasswordRequestCmd : iResetPasswordRequestCmd
    {
        #region C'tor
        public ResetPasswordRequestCmd(string userName)
        {
            UserName = userName;
        }
        #endregion

        #region Interface iResetPasswordRequestCmd
        /// <summary>
        /// Original filename of uploaded file
        /// </summary>
        public string UserName { get; private set; }
        #endregion
    }
}
