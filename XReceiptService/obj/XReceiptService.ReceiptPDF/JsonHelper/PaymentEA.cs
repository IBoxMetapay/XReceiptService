using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace XReceiptService.ReceiptPDF.JsonHelper
{
    public class PaymentEA
    {
        public Payment payment { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Payment
    {
        public agentInfo agentInfo { get; set; }
        public authCode authCode { get; set; }
        public banksAgentEdrpou banksAgentEdrpou { get; set; }
        public banksAgentName banksAgentName { get; set; }
        public contactsInfo contactsInfo { get; set; }
        public controlNumber controlNumber { get; set; }
        public currency currency { get; set; }
        public fiscalCheq fiscalCheq { get; set; }
        public fiscalInfo fiscalInfo { get; set; }
        public formPay formPay { get; set; }
        public modeTaxNum modeTaxNum { get; set; }
        public int? needFiscalInfo { get; set; }
        public operationId operationId { get; set; }
        public operationSpecies operationSpecies { get; set; }
        public operationSum operationSum { get; set; }
        public List<List<Dictionary<string, KeyValue>>> operatorInfo { get; set; }
        public orderTaxNum orderTaxNum { get; set; }
        public pDv pdv { get; set; }
        public platTaxNum platTaxNum { get; set; }
        public receiptId receiptId { get; set; }
        public List<recipients> recipients { get; set; }
        public repostReceiptId repostReceiptId { get; set; }
        public rrn rrn { get; set; }
        public rroFiscalNum rroFiscalNum { get; set; }
        public salePoint salePoint { get; set; }
        public salePointAddr salePointAddr { get; set; }
        public List<sender> sender { get; set; }
        public senderBankEdrpou senderBankEdrpou { get; set; }
        public senderBankMfo senderBankMfo { get; set; }
        public senderBankName senderBankName { get; set; }
        public int? serviceId { get; set; }
        public serviceSum serviceSum { get; set; }
        public stateId stateId { get; set; }
        public status status { get; set; }
        public taxSum taxSum { get; set; }
        public string templateOperatorInfo { get; set; }
        public txnDate txnDate { get; set; }
    }

    public class agentInfo
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class authCode
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class banksAgentEdrpou
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class banksAgentName
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class contactsInfo
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class controlNumber
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class currency
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class fiscalCheq
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class fiscalInfo
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class formPay
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class modeTaxNum
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class operationId
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class operationSpecies
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class operationSum
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class orderTaxNum
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }
    public class pDv
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class platTaxNum
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class receiptId
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class recipients
    {
        public purpose purpose { get; set; }
        public List<recipient> recipient { get; set; }
        public serviceName serviceName { get; set; }
    }

    public class repostReceiptId
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class rrn
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class rroFiscalNum
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class salePoint
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class salePointAddr
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class sender
    {
        public int? id { get; set; }
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class senderBankEdrpou
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class senderBankMfo
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class senderBankName
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class serviceId
    {
        public int ServiceId { get; set; }
    }

    public class serviceSum
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class stateId
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class status
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class taxSum
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class templateOperatorInfo
    {
        public string TemplateOperatorInfo { get; set; }
    }

    public class txnDate
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class recipient
    {
        public int? id { get; set; }
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class purpose
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }

    public class serviceName
    {
        public string label { get; set; }
        public ElementType type { get; set; }
        public string value { get; set; }
    }
    public class KeyValue
    {
        public string label { get; set; }
        public ElementType _type { get; set; }
        public string type
        {
            get { return _type.ToString(); }
            set
            {
                if (!string.IsNullOrEmpty(value) && value.ToUpper() == "QR")
                    _type = ElementType.QR;
                if (!string.IsNullOrEmpty(value) && value.Contains("Link"))
                    _type = ElementType.Link;
            }
        }
        public string value { get; set; }
    }

    public enum ElementType
    {
        Text = 0,
        QR = 1,
        Link = 2
    }
}