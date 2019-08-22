﻿using GlobalPayments.Api.Terminals.Abstractions;

namespace GlobalPayments.Api.Terminals {
    public abstract class TerminalResponse : IDeviceResponse {
        // Internal

        /// <summary>
        /// device status at the time of transaction
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// the command used in the transaction
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// the version of software the terminal is running
        /// </summary>
        public string Version { get; set; }

        // Functional
        /// <summary>
        /// response code returned by the device
        /// </summary>
        public string DeviceResponseCode { get; set; }

        /// <summary>
        /// response text returned by the device
        /// </summary>
        public string DeviceResponseText { get; set; }

        /// <summary>
        /// response code returned by the gateway
        /// </summary>
        public string ResponseCode { get; set; }

        /// <summary>
        /// response message returned by the gateway
        /// </summary>
        public string ResponseText { get; set; }

        /// <summary>
        /// the gateway transaction id
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// the device's transaction reference number
        /// </summary>
        public string TerminalRefNumber { get; set; }

        /// <summary>
        /// the multi-use payment token generated by the device in instances where tokenization is requested
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// value indicating the presence/non-presence of signature data 
        /// </summary>
        public string SignatureStatus { get; set; }

        /// <summary>
        /// byte array containing the bitmap data for the signature (you may have to call GetSignature depending on your device)
        /// </summary>
        public byte[] SignatureData { get; set; }

        // Transactional
        /// <summary>
        /// the type of transaction (Sale, Authorization, Verify etc...)
        /// </summary>
        public string TransactionType { get; set; }

        /// <summary>
        /// the masked credit card number
        /// </summary>
        public string MaskedCardNumber { get; set; }

        /// <summary>
        /// value denoting whether a card was swiped, inserted, tapped or manually entered
        /// </summary>
        public string EntryMethod { get; set; }

        /// <summary>
        /// the authorization code returned by the issuer
        /// </summary>
        public string AuthorizationCode { get; set; }

        /// <summary>
        /// the approval code issued by the device
        /// </summary>
        public string ApprovalCode { get; set; }

        /// <summary>
        /// the amount of the transaction
        /// </summary>
        public decimal? TransactionAmount { get; set; }

        /// <summary>
        /// the remaining balance in instances of partial approval or gift sales
        /// </summary>
        public decimal? AmountDue { get; set; }

        /// <summary>
        /// the balance of a prepaid or gift card when running a balance inquiry
        /// </summary>
        public decimal? BalanceAmount { get; set; }

        /// <summary>
        /// the card holder name as represented in the track data
        /// </summary>
        public string CardHolderName { get; set; }

        /// <summary>
        /// the BIN range of the card used
        /// </summary>
        public string CardBIN { get; set; }

        /// <summary>
        /// flag indicating whether or not the card was present during the transaction
        /// </summary>
        public bool CardPresent { get; set; }

        /// <summary>
        /// card expiration date
        /// </summary>
        public string ExpirationDate { get; set; }

        /// <summary>
        /// the tip amount applied to the transaction if any
        /// </summary>
        public decimal? TipAmount { get; set; }

        /// <summary>
        /// the cash back amount requested during a debit transaction
        /// </summary>
        public decimal? CashBackAmount { get; set; }

        /// <summary>
        /// Response code from the address verification system
        /// </summary>
        public string AvsResponseCode { get; set; }

        /// <summary>
        /// response text from the address verification system
        /// </summary>
        public string AvsResponseText { get; set; }

        /// <summary>
        /// response code from the CVN/CVV Check.
        /// </summary>
        public string CvvResponseCode { get; set; }

        /// <summary>
        /// response text from the CVN/CVV Check
        /// </summary>
        public string CvvResponseText { get; set; }

        /// <summary>
        /// For level II transactions, value indicating tax exemption status
        /// </summary>
        public bool TaxExempt { get; set; }

        /// <summary>
        /// For level II the business tax exemption ID
        /// </summary>
        public string TaxExemptId { get; set; }

        /// <summary>
        /// The ticket number associated with the transaction
        /// </summary>
        public string TicketNumber { get; set; }

        /// <summary>
        /// The type of payment method used (Credit, Debit, etc...)
        /// </summary>
        public string PaymentType { get; set; }

        // EMV
        /// <summary>
        /// The preferred name of the EMV application selected on the EMV card
        /// </summary>
        public string ApplicationPreferredName { get; set; }

        /// <summary>
        /// The aplication label from the EMV card
        /// </summary>
        public string ApplicationLabel { get; set; }

        /// <summary>
        /// the AID (Application ID) of the selected application on the EMV card
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// The cryptogram type used during the transaction
        /// </summary>
        public ApplicationCryptogramType ApplicationCryptogramType { get; set; }

        /// <summary>
        /// The actual cryptogram value generated for the transaction
        /// </summary>
        public string ApplicationCryptogram { get; set; }

        /// <summary>
        /// The CVM used in the transaction (PIN, Signature, etc...)
        /// </summary>
        public string CardHolderVerificationMethod { get; set; }

        /// <summary>
        /// The results of the terminals attempt to verify the cards authenticity.
        /// </summary>
        public string TerminalVerificationResults { get; set; }
    }

    public enum ApplicationCryptogramType {
        TC,
        ARQC
    }
}
