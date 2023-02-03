using HtmlAgilityPack;
using Net.Codecrete.QrCodeGenerator;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using XReceiptService.ReceiptPDF.JsonHelper;

namespace XReceiptService.ReceiptPDF
{
    public class UniversalMacrosReplace
    {
        public List<string> DataToPdf(ReceiptResp payment, string HtmlExample)
        {
            var res = new List<string>();
            var ValuesToReplace = new Dictionary<string, string>();
            var NodesToDelete = new List<HtmlNode>();

            foreach (var item in payment.payment)
            {
                var htmlText = HtmlExample;
                HtmlDocument ChangedHtml = new HtmlDocument();
                ChangedHtml.LoadHtml(htmlText);

                #region [Simple macros replace]

                ValuesToReplace.Add("%operatorInfo%", string.Empty);
                ValuesToReplace.Add("%salePointId%", payment.salePointId?.ToString());
                ValuesToReplace.Add("%serviceNameAll%", payment.serviceName);
                ValuesToReplace.Add("%operationSumAll%", payment.operationSumAll);
                ValuesToReplace.Add("%serviceSumAll%", payment.serviceSumAll);
                ValuesToReplace.Add("%taxSumAll%", payment.taxSumAll);
                ValuesToReplace.Add("%currencyAll%", payment.currencyAll);
                ValuesToReplace.Add("%receiptId%", payment.receiptId);
                ValuesToReplace.Add("%receiptDate%", DateTime.Now.ToString(new CultureInfo("ru-Ru")));

                if (item != null && item.repostReceiptId != null)
                {
                    if (item.repostReceiptId.value != null && !string.IsNullOrEmpty(item.repostReceiptId.value))
                    {
                        ValuesToReplace.Add("%receiptIdNoSplit%", item.repostReceiptId.value.Replace("-", ""));
                        ValuesToReplace.Add("%repostReceiptId.value%", item.repostReceiptId.value);
                    }
                    else
                    {
                        ValuesToReplace.Add("%repostReceiptId.value%", string.Empty);
                        ValuesToReplace.Add("%receiptIdNoSplit%", string.Empty);
                    }
                }
                else
                {
                    ValuesToReplace.Add("%repostReceiptId.value%", string.Empty);
                    ValuesToReplace.Add("%receiptIdNoSplit%", string.Empty);
                }

                if (item != null && item.operationId != null)
                {
                    if (item.operationId.label == null && item.operationId.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='operationId']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%operationId.label%", string.Empty);
                        ValuesToReplace.Add("%operationId.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%operationId.label%", item.operationId.label);
                        ValuesToReplace.Add("%operationId.value%", item.operationId.value);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='operationId']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%operationId.label%", string.Empty);
                    ValuesToReplace.Add("%operationId.value%", string.Empty);
                }

                if(item != null && (item.platTaxNum == null || item.modeTaxNum == null || item.operationSpecies == null || item.formPay == null || item.rroFiscalNum == null || item.orderTaxNum == null))
                {
                    DeleteFiscalData(item, ChangedHtml, ValuesToReplace, NodesToDelete);
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='fiscalData']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                }

                if (item != null && item.banksAgentName != null)
                {
                    if (item.banksAgentName.label == null && item.banksAgentName.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='banksAgentName']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%banksAgentName.label%", string.Empty);
                        ValuesToReplace.Add("%banksAgentName.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%banksAgentName.value%", item.banksAgentName.value);
                        ValuesToReplace.Add("%banksAgentName.label%", item.banksAgentName.label);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='banksAgentName']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%banksAgentName.label%", string.Empty);
                    ValuesToReplace.Add("%banksAgentName.value%", string.Empty);
                }

                if (item != null && item.salePoint != null)
                {
                    if (item.salePoint.label == null && item.salePoint.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='salePoint']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%salePoint.label%", string.Empty);
                        ValuesToReplace.Add("%salePoint.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%salePoint.label%", item.salePoint.label);
                        ValuesToReplace.Add("%salePoint.value%", item.salePoint.value);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='salePoint']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%salePoint.label%", string.Empty);
                    ValuesToReplace.Add("%salePoint.value%", string.Empty);
                }

                if (item != null && item.salePointAddr != null)
                {
                    if (item.salePointAddr.label == null && item.salePointAddr.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='salePointAddr']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%salePointAddr.label%", string.Empty);
                        ValuesToReplace.Add("%salePointAddr.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%salePointAddr.label%", item.salePointAddr.label);
                        ValuesToReplace.Add("%salePointAddr.value%", item.salePointAddr.value);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='salePointAddr']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%salePointAddr.label%", string.Empty);
                    ValuesToReplace.Add("%salePointAddr.value%", string.Empty);
                }

                if (item != null && item.senderBankName != null)
                {
                    if (item.senderBankName.label == null && item.senderBankName.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='senderBankName']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%senderBankName.label%", string.Empty);
                        ValuesToReplace.Add("%senderBankName.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%senderBankName.label%", item.senderBankName.label);
                        ValuesToReplace.Add("%senderBankName.value%", item.senderBankName.value);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='senderBankName']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%senderBankName.label%", string.Empty);
                    ValuesToReplace.Add("%senderBankName.value%", string.Empty);
                }

                if (item != null && item.senderBankEdrpou != null)
                {
                    if (item.senderBankEdrpou.label == null && item.senderBankEdrpou.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='senderBankEdrpou']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%senderBankEdrpou.label%", string.Empty);
                        ValuesToReplace.Add("%senderBankEdrpou.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%senderBankEdrpou.label%", item.senderBankEdrpou.label);
                        ValuesToReplace.Add("%senderBankEdrpou.value%", item.senderBankEdrpou.value);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='senderBankEdrpou']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%senderBankEdrpou.label%", string.Empty);
                    ValuesToReplace.Add("%senderBankEdrpou.value%", string.Empty);
                }

                if (item != null && item.senderBankMfo != null)
                {
                    if (item.senderBankMfo.label == null && item.senderBankMfo.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='senderBankMfo']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%senderBankMfo.label%", string.Empty);
                        ValuesToReplace.Add("%senderBankMfo.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%senderBankMfo.label%", item.senderBankMfo.label);
                        ValuesToReplace.Add("%senderBankMfo.value%", item.senderBankMfo.value);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='senderBankMfo']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%senderBankMfo.label%", string.Empty);
                    ValuesToReplace.Add("%senderBankMfo.value%", string.Empty);
                }

                if (item != null && item.banksAgentEdrpou != null)
                {
                    if (item.banksAgentEdrpou.label == null && item.banksAgentEdrpou.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='banksAgentEdrpou']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%banksAgentEdrpou.label%", string.Empty);
                        ValuesToReplace.Add("%banksAgentEdrpou.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%banksAgentEdrpou.label%", item.banksAgentEdrpou.label);
                        ValuesToReplace.Add("%banksAgentEdrpou.value%", item.banksAgentEdrpou.value);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='banksAgentEdrpou']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%banksAgentEdrpou.label%", string.Empty);
                    ValuesToReplace.Add("%banksAgentEdrpou.value%", string.Empty);
                }

                if (item != null && item.rrn != null)
                {
                    if (item.rrn.label == null && item.rrn.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='rrn']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%rrn.label%", string.Empty);
                        ValuesToReplace.Add("%rrn.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%rrn.label%", item.rrn.label);
                        ValuesToReplace.Add("%rrn.value%", item.rrn.value);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='rrn']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%rrn.label%", string.Empty);
                    ValuesToReplace.Add("%rrn.value%", string.Empty);
                }

                if (item != null && item.authCode != null)
                {
                    if (item.authCode.label == null && item.authCode.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='authCode']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%authCode.label%", string.Empty);
                        ValuesToReplace.Add("%authCode.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%authCode.label%", item.authCode.label);
                        ValuesToReplace.Add("%authCode.value%", item.authCode.value);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='authCode']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%authCode.label%", string.Empty);
                    ValuesToReplace.Add("%authCode.value%", string.Empty);
                }

                if (item != null && item.currency != null)
                {
                    if (item.currency.label == null && item.currency.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='currency']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%currency.label%", string.Empty);
                        ValuesToReplace.Add("%currency.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%currency.label%", item.currency.label);
                        ValuesToReplace.Add("%currency.value%", string.Empty);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='currency']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%currency.label%", string.Empty);
                    ValuesToReplace.Add("%currency.value%", string.Empty);
                }

                if (item != null && item.operationSum != null)
                {
                    if (item.operationSum.label == null && item.operationSum.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='operationSum']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%operationSum.label%", string.Empty);
                        ValuesToReplace.Add("%operationSum.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%operationSum.label%", item.operationSum.label);
                        ValuesToReplace.Add("%operationSum.value%", item.operationSum.value);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='operationSum']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%operationSum.label%", string.Empty);
                    ValuesToReplace.Add("%operationSum.value%", string.Empty);
                }

                if (item != null && item.serviceSum != null)
                {
                    if (item.serviceSum.label == null && item.serviceSum.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='serviceSum']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%serviceSum.label%", string.Empty);
                        ValuesToReplace.Add("%serviceSum.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%serviceSum.label%", item.serviceSum.label);
                        ValuesToReplace.Add("%serviceSum.value%", item.serviceSum.value);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='serviceSum']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%serviceSum.label%", string.Empty);
                    ValuesToReplace.Add("%serviceSum.value%", string.Empty);
                }

                if (item != null && item.taxSum != null)
                {
                    if (item.taxSum.label == null && item.taxSum.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='taxSum']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%taxSum.label%", string.Empty);
                        ValuesToReplace.Add("%taxSum.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%taxSum.label%", item.taxSum.label);
                        ValuesToReplace.Add("%taxSum.value%", item.taxSum.value);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='taxSum']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%taxSum.label%", string.Empty);
                    ValuesToReplace.Add("%taxSum.value%", string.Empty);
                }

                if (item != null && item.txnDate != null)
                {
                    if (item.txnDate.label == null && item.txnDate.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='txnDate']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%txnDate.label%", string.Empty);
                        ValuesToReplace.Add("%txnDate.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%txnDate.label%", item.txnDate.label);
                        ValuesToReplace.Add("%txnDate.value%", item.txnDate.value);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='txnDate']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%txnDate.label%", string.Empty);
                    ValuesToReplace.Add("%txnDate.value%", string.Empty);
                }

                if (item != null && item.status != null)
                {
                    if (item.status.label == null && item.status.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='status']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%status.label%", string.Empty);
                        ValuesToReplace.Add("%status.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%status.label%", item.status.label);
                        ValuesToReplace.Add("%status.value%", item.status.value);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='status']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%status.label%", string.Empty);
                    ValuesToReplace.Add("%status.value%", string.Empty);
                }

                if (item != null && item.receiptId != null)
                {
                    if (item.receiptId.label == null && item.receiptId.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='receiptId']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%receiptId.label%", string.Empty);
                        ValuesToReplace.Add("%receiptId.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%receiptId.label%", item.receiptId.label);
                        ValuesToReplace.Add("%receiptId.value%", item.receiptId.value);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='receiptId']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%receiptId.label%", string.Empty);
                    ValuesToReplace.Add("%receiptId.value%", string.Empty);
                }

                if (item != null && item.stateId != null)
                {
                    if (item.stateId.label == null && item.stateId.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='stateId']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%stateId.label%", string.Empty);
                        ValuesToReplace.Add("%stateId.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%stateId.label%", item.stateId.label);
                        ValuesToReplace.Add("%stateId.value%", item.stateId.value);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='stateId']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%stateId.label%", string.Empty);
                    ValuesToReplace.Add("%stateId.value%", string.Empty);
                }

                if (item != null && item.agentInfo != null)
                {
                    if (item.agentInfo.label == null && item.agentInfo.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='agentInfo']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%agentInfo.label%", string.Empty);
                        ValuesToReplace.Add("%agentInfo.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%agentInfo.label%", item.agentInfo.label);
                        ValuesToReplace.Add("%agentInfo.value%", item.agentInfo.value);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='agentInfo']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%agentInfo.label%", string.Empty);
                    ValuesToReplace.Add("%agentInfo.value%", string.Empty);
                }

                if (item != null && item.contactsInfo != null)
                {
                    if (item.contactsInfo.label == null && item.contactsInfo.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='contactsInfo']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%contactsInfo.label%", string.Empty);
                        ValuesToReplace.Add("%contactsInfo.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%contactsInfo.label%", item.contactsInfo.label);
                        ValuesToReplace.Add("%contactsInfo.value%", item.contactsInfo.value);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='contactsInfo']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%contactsInfo.label%", string.Empty);
                    ValuesToReplace.Add("%contactsInfo.value%", string.Empty);
                }

                if (item != null && item.controlNumber != null)
                {
                    if (item.controlNumber.label == null && item.controlNumber.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='controlNumber']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%controlNumber.label%", string.Empty);
                        ValuesToReplace.Add("%controlNumber.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%controlNumber.label%", item.controlNumber.label);
                        ValuesToReplace.Add("%controlNumber.value%", item.controlNumber.value);
                    }
                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='controlNumber']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%controlNumber.label%", string.Empty);
                    ValuesToReplace.Add("%controlNumber.value%", string.Empty);
                }

                if (item != null && item.pdv != null)
                {
                    if (item.pdv.label == null && item.pdv.value == null)
                    {
                        var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='pdv']");
                        if (toDelete != null)
                            NodesToDelete.Add(toDelete);
                        ValuesToReplace.Add("%pdv.label%", string.Empty);
                        ValuesToReplace.Add("%pdv.value%", string.Empty);
                    }
                    else
                    {
                        ValuesToReplace.Add("%pdv.label%", item.pdv.label);
                        ValuesToReplace.Add("%pdv.value%", item.pdv.value);
                    }

                }
                else
                {
                    var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='pdv']");
                    if (toDelete != null)
                        NodesToDelete.Add(toDelete);
                    ValuesToReplace.Add("%pdv.label%", string.Empty);
                    ValuesToReplace.Add("%pdv.value%", string.Empty);
                }

                #endregion

                var toPdf = string.Empty;
                var senderToPdf = string.Empty;

                foreach (var k in item.recipients)
                {
                    var TableTest = "<table style=\"width:700px;\"><tr><td style=\"width:40%;vertical-align:top;\">";

                    if (k.serviceName != null && k.serviceName.label != null && k.serviceName.value != null)
                    {
                        if (k.serviceName.value.Length > 40)
                        {
                            string valueInfo = string.Empty;
                            int start = 0, end;
                            var lines = new List<string>();
                            k.serviceName.value = Regex.Replace(k.serviceName.value, @"\s", " ").Trim();

                            while ((end = start + 31) < k.serviceName.value.Length)
                            {
                                while (k.serviceName.value[end] != ' ' && end > start)
                                    end -= 1;

                                if (end == start)
                                    end = start + 31;

                                lines.Add(k.serviceName.value.Substring(start, end - start));
                                start = end + 1;
                            }

                            if (start < k.serviceName.value.Length)
                                lines.Add(k.serviceName.value.Substring(start));

                            foreach (var c in lines)
                                valueInfo += c + "\n";
                            TableTest += $"<div><span><b>{k.serviceName.label}</b></span> {valueInfo}</div>";
                            lines.Clear();
                        }
                        else
                            TableTest += $"<div><span><b>{k.serviceName.label}</b></span> {k.serviceName.value}</div>";
                    }

                    foreach (var v in k.recipient)
                    {
                        if (v.id != 23 && v.id != 22 && v.id != 21 && v.id != 24 && v.id != 54)
                        {
                            if (v.value.Length > 40)
                            {
                                string valueInfo = string.Empty;
                                int start = 0, end;
                                var lines = new List<string>();
                                v.value = Regex.Replace(v.value, @"\s", " ").Trim();

                                while ((end = start + 31) < v.value.Length)
                                {
                                    while (v.value[end] != ' ' && end > start)
                                        end -= 1;

                                    if (end == start)
                                        end = start + 31;

                                    lines.Add(v.value.Substring(start, end - start));
                                    start = end + 1;
                                }

                                if (start < v.value.Length)
                                    lines.Add(v.value.Substring(start));

                                foreach (var c in lines)
                                    valueInfo += c + "\n";
                                TableTest += $"<div><span><b>{v.label}</b></span> {valueInfo}</div>";
                                lines.Clear();
                            }
                            else
                                TableTest += $"<div><span><b>{v.label}</b></span> {v.value}</div>";
                        }
                    }

                    if (k.purpose != null && k.purpose.label != null && k.purpose.value != null)
                    {
                        if (k.purpose.value.Length > 40)
                        {
                            string valueInfo = string.Empty;
                            int start = 0, end;
                            var lines = new List<string>();
                            k.purpose.value = Regex.Replace(k.purpose.value, @"\s", " ").Trim();

                            while ((end = start + 31) < k.purpose.value.Length)
                            {
                                while (k.purpose.value[end] != ' ' && end > start)
                                    end -= 1;

                                if (end == start)
                                    end = start + 33;

                                lines.Add(k.purpose.value.Substring(start, end - start));
                                start = end + 1;
                            }

                            if (start < k.purpose.value.Length)
                                lines.Add(k.purpose.value.Substring(start));

                            foreach (var c in lines)
                                valueInfo += c + "\n";
                            TableTest += $"<div><span><b>{k.purpose.label}</b></span> {valueInfo}</div>";
                            lines.Clear();
                        }
                        else
                            TableTest += $"<div><span><b>{k.purpose.label}</b></span> {k.purpose.value}</div>";
                    }

                    TableTest += "</td>" +
                        "<td style=\"vertical-align:top;width:5%\"></td>" +
                        "<td style=\"width:45%;vertical-align:top;\">";

                    foreach (var l in k.recipient)
                    {
                        if (l.label == null && l.value == null)
                            continue;
                        if (l.id == 23)
                        {
                            TableTest += $"<div><span><b>{l.label}</b></span> {l.value}</div>";
                            continue;
                        }
                        if (l.id == 22)
                        {
                            TableTest += $"<div><span><b>{l.label}</b></span> {l.value}</div>";
                            continue;
                        }
                        if (l.id == 21)
                        {
                            TableTest += $"<div><span><b>{l.label}</b></span> {l.value}</div>";
                            continue;
                        }
                        if (l.id == 24)
                        {
                            TableTest += $"<div><span><b>{l.label}</b></span> {l.value}</div>";
                            continue;
                        }
                        if (l.id == 54)
                        {
                            if (l.value.Length > 40)
                            {
                                string valueInfo = string.Empty;
                                int start = 0, end;
                                var lines = new List<string>();
                                l.value = Regex.Replace(l.value, @"\s", " ").Trim();

                                while ((end = start + 31) < l.value.Length)
                                {
                                    while (l.value[end] != ' ' && end > start)
                                        end -= 1;

                                    if (end == start)
                                        end = start + 31;

                                    lines.Add(l.value.Substring(start, end - start));
                                    start = end + 1;
                                }

                                if (start < l.value.Length)
                                    lines.Add(l.value.Substring(start));

                                foreach (var c in lines)
                                    valueInfo += c + "\n";
                                TableTest += $"<div><span><b>{l.label}</b></span> {valueInfo}</div>";

                                lines.Clear();
                                continue;
                            }
                            else
                                TableTest += $"<div><span><b>{l.label}</b></span> {l.value}</div>";
                            continue;
                        }
                    }

                    TableTest += "</td></tr></table>";
                    toPdf += TableTest;
                }

                if (item.sender != null)
                {
                    var TableTest = "<table style=\"width:315px;\"><tr><td style=\"width:50%;vertical-align:top;\">";
                    foreach (var senderdata in item.sender)
                    {
                        if (senderdata.label == null && senderdata.value == null)
                            continue;
                        else
                        {
                            if (senderdata.value.Length > 40)
                            {
                                string valueInfo = string.Empty;
                                int start = 0, end;
                                var lines = new List<string>();
                                senderdata.value = Regex.Replace(senderdata.value, @"\s", " ").Trim();

                                while ((end = start + 31) < senderdata.value.Length)
                                {
                                    while (senderdata.value[end] != ' ' && end > start)
                                        end -= 1;

                                    if (end == start)
                                        end = start + 31;

                                    lines.Add(senderdata.value.Substring(start, end - start));
                                    start = end + 1;
                                }

                                if (start < senderdata.value.Length)
                                    lines.Add(senderdata.value.Substring(start));

                                foreach (var c in lines)
                                    valueInfo += c + "\n";
                                TableTest += $"<div><span><b>{senderdata.label}</b></span> {valueInfo}</div>";
                                lines.Clear();
                            }
                            else
                                TableTest += $"<div><span><b>{senderdata.label}</b></span> {senderdata.value}</div>";
                        }
                    }
                    TableTest += "</td></tr></table>";
                    senderToPdf += TableTest;
                }

                ValuesToReplace.Add($"%recepient%", toPdf);
                ValuesToReplace.Add($"%sender%", senderToPdf);

                foreach (var deleteThis in NodesToDelete)
                    deleteThis.Remove();

                StringBuilder smsRemplate = new StringBuilder(ChangedHtml.DocumentNode.InnerHtml);
                foreach (var p in ValuesToReplace)
                {
                    if (p.Key == "%recepient%")
                    {
                        smsRemplate.Replace(p.Key, p.Value);
                        ValuesToReplace.Remove("%recepient%");
                    }
                    if (p.Key == "%sender%")
                    {
                        smsRemplate.Replace(p.Key, p.Value);
                        ValuesToReplace.Remove("%sender%");
                    }
                }

                foreach (var p in ValuesToReplace)
                    smsRemplate.Replace(p.Key, p.Value);

                res.Add(smsRemplate.ToString());
                ValuesToReplace.Clear();
            }
            return res;
        }

        public List<string> OperInfoDataToPdf(ReceiptResp payment)
        {
            // list to return data to pdf
            var res = new List<string>();

            var operInfTemp = "";

            List<List<Dictionary<string, KeyValue>>> listOperInfo = new List<List<Dictionary<string, KeyValue>>>();

            //check if operatorInfo is null or empty
            foreach (var c in payment.payment)
            {
                if (c.operatorInfo == null)
                    continue;
                if (c.operatorInfo != null || c.operatorInfo.Count != 0)
                    listOperInfo = c.operatorInfo;
            }

            //check if template is null or empty
            foreach (var c in payment.payment)
            {
                if (c.templateOperatorInfo == null || string.IsNullOrEmpty(c.templateOperatorInfo))
                    continue;
                else
                    operInfTemp = c.templateOperatorInfo;
            }

            //list to replace values in template
            List<(string, string)> ValuesToReplace = new List<(string, string)>();
            string WithTemplates_data = operInfTemp;
            List<string> WithTemplates = new List<string>();
            //get instance of template from payment
            StringBuilder smsRemplate = new StringBuilder(operInfTemp);

            int j = 0;

            for (int i = 0; i < listOperInfo.Count; i++)
            {
                WithTemplates.Add(WithTemplates_data);
                res.Add("");
            }

            if (listOperInfo.Count > 1)
            {
                foreach (var c in listOperInfo)
                {
                    foreach (var h in c)
                    {
                        foreach (var data in h)
                        {
                            if (data.Value != null)
                            {
                                if ((data.Value.label == null || string.IsNullOrEmpty(data.Value.label)) && (data.Value.value == null || string.IsNullOrEmpty(data.Value.value)))
                                {
                                    ValuesToReplace.Add(($"%{data.Key}.label%", string.Empty));
                                    ValuesToReplace.Add(($"%{data.Key}.value%", string.Empty));
                                    continue;
                                }
                            }

                            if (WithTemplates[j] != null && WithTemplates[j].Contains(data.Key + ".label"))
                            {
                                if (data.Value.label != null)
                                    ValuesToReplace.Add(($"%{data.Key}.label%", data.Value.label));
                                else
                                    ValuesToReplace.Add(($"%{data.Key}.label%", string.Empty));
                            }
                            else
                                ValuesToReplace.Add(($"%{data.Key}.label%", string.Empty));

                            if (WithTemplates[j] != null && WithTemplates[j].Contains(data.Key + ".value"))
                            {
                                if (data.Value.label != null && data.Value.value != null)
                                {
                                    if (data.Value._type != null && data.Value._type == ElementType.QR)
                                    {

                                        var qr = QrCode.EncodeText(data.Value.value, QrCode.Ecc.Medium);
                                        string base64 = "";
                                        using (var bitmap = qr.ToBitmap(4, 10))
                                        {
                                            using (var memoryStream = new MemoryStream())
                                            {
                                                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                                base64 = Convert.ToBase64String(memoryStream.ToArray());
                                            }

                                        }
                                        ValuesToReplace.Add(($"%{data.Key}.value%", "<img src=\"data:image/png;base64,\n" + base64 + "\" alt=\"\">"));
                                    }
                                    else if (data.Value._type != null && data.Value._type == ElementType.Link)
                                    {
                                        ValuesToReplace.Add(($"%{data.Key}.value%", $"<a href=\"{data.Value.value}\">{data.Value.value}</a>"));
                                    }
                                    else
                                        ValuesToReplace.Add(($"%{data.Key}.value%", data.Value.value));
                                }
                                else
                                {
                                    ValuesToReplace.Add(($"%{data.Key}.label%", string.Empty));
                                    ValuesToReplace.Add(($"%{data.Key}.value%", string.Empty));
                                    continue;
                                }
                            }
                            else
                                ValuesToReplace.Add(($"%{data.Key}.label%", string.Empty));
                        }
                    }

                    foreach (var p in ValuesToReplace)
                    {
                        smsRemplate.Replace(p.Item1, p.Item2);
                    }
                    ValuesToReplace.Clear();
                    res[j] = smsRemplate.ToString();
                    smsRemplate = new StringBuilder(operInfTemp);

                    j++;
                }
            }
            else
            {
                foreach (var c in listOperInfo)
                {
                    foreach (var h in c)
                    {
                        foreach (var data in h)
                        {
                            if (data.Value != null)
                            {
                                if ((data.Value.label == null || string.IsNullOrEmpty(data.Value.label)) && (data.Value.value == null || string.IsNullOrEmpty(data.Value.value)))
                                {
                                    ValuesToReplace.Add(($"%{data.Key}.label%", string.Empty));
                                    ValuesToReplace.Add(($"%{data.Key}.value%", string.Empty));
                                    continue;
                                }
                            }

                            if (WithTemplates[j] != null && WithTemplates[j].Contains(data.Key + ".label"))
                            {
                                if (data.Value.label != null && data.Value.value != null)
                                    ValuesToReplace.Add(($"%{data.Key}.label%", data.Value.label));
                                else
                                    ValuesToReplace.Add(($"%{data.Key}.label%", string.Empty));
                            }
                            else
                                ValuesToReplace.Add(($"%{data.Key}.label%", string.Empty));

                            if (WithTemplates[j] != null && WithTemplates[j].Contains(data.Key + ".value"))
                            {
                                if (data.Value.label != null && data.Value.value != null)
                                {
                                    if (data.Value._type != null && data.Value._type == ElementType.QR)
                                    {

                                        var qr = QrCode.EncodeText(data.Value.value, QrCode.Ecc.Medium);
                                        string base64 = "";
                                        using (var bitmap = qr.ToBitmap(4, 10))
                                        {
                                            using (var memoryStream = new MemoryStream())
                                            {
                                                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                                base64 = Convert.ToBase64String(memoryStream.ToArray());
                                            }

                                        }
                                        ValuesToReplace.Add(($"%{data.Key}.value%", "<img src=\"data:image/png;base64,\n" + base64 + "\" alt=\"\">"));
                                    }
                                    else if (data.Value._type != null && data.Value._type == ElementType.Link)
                                    {
                                        ValuesToReplace.Add(($"%{data.Key}.value%", $"<a href=\"{data.Value.value}\">{data.Value.value}</a>"));
                                    }
                                    else
                                        ValuesToReplace.Add(($"%{data.Key}.value%", data.Value.value));
                                }
                                else
                                {
                                    ValuesToReplace.Add(($"%{data.Key}.label%", string.Empty));
                                    ValuesToReplace.Add(($"%{data.Key}.value%", string.Empty));
                                    continue;
                                }
                            }
                            else
                                ValuesToReplace.Add(($"%{data.Key}.label%", string.Empty));
                        }
                    }

                    foreach (var p in ValuesToReplace)
                    {
                        smsRemplate.Replace(p.Item1, p.Item2);
                    }
                    ValuesToReplace.Clear();
                    res[j] = smsRemplate.ToString();
                    smsRemplate = new StringBuilder(operInfTemp);
                }
            }

            var concatedoperInf = new List<string>();
            var toconcated = string.Empty;
            var getLayout = getBetween(operInfTemp, ">", "</layout_page>");
            if (getLayout != null && !string.IsNullOrEmpty(getLayout))
            {
                switch (getLayout)
                {
                    case "0":
                        foreach (var c in res)
                            toconcated += c + "<br><br>";
                        concatedoperInf.Add(toconcated);
                        return concatedoperInf;
                    default:
                        break;
                }
            }

            if (getLayout == null || string.IsNullOrEmpty(getLayout))
            {
                foreach (var c in res)
                {
                    if (c != null)
                        toconcated += c + "<br><br>";
                }
                concatedoperInf.Add(toconcated);
                return concatedoperInf;
            }

            return res;
        }

        public string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }

        public void DeleteFiscalData(Payment item, HtmlDocument ChangedHtml, Dictionary<string, string> ValuesToReplace, List<HtmlNode> NodesToDelete)
        {
            var toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='fiscalInfo']");
            if (toDelete != null)
                NodesToDelete.Add(toDelete);
            ValuesToReplace.Add("%fiscalInfo.label%", string.Empty);
            ValuesToReplace.Add("%fiscalInfo.value%", string.Empty);

            toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='fiscalCheq']");
            if (toDelete != null)
                NodesToDelete.Add(toDelete);
            ValuesToReplace.Add("%fiscalCheq.label%", string.Empty);
            ValuesToReplace.Add("%fiscalCheq.value%", string.Empty);

            toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='operationSpecies']");
            if (toDelete != null)
                NodesToDelete.Add(toDelete);
            ValuesToReplace.Add("%operationSpecies.label%", string.Empty);
            ValuesToReplace.Add("%operationSpecies.value%", string.Empty);

            toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='formPay']");
            if (toDelete != null)
                NodesToDelete.Add(toDelete);
            ValuesToReplace.Add("%formPay.label%", string.Empty);
            ValuesToReplace.Add("%formPay.value%", string.Empty);

            toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='modeTaxNum']");
            if (toDelete != null)
                NodesToDelete.Add(toDelete);
            ValuesToReplace.Add("%modeTaxNum.label%", string.Empty);
            ValuesToReplace.Add("%modeTaxNum.value%", string.Empty);
           
            toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='orderTaxNum']");
            if (toDelete != null)
                NodesToDelete.Add(toDelete);
            ValuesToReplace.Add("%orderTaxNum.label%", string.Empty);
            ValuesToReplace.Add("%orderTaxNum.value%", string.Empty);
            
            toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='platTaxNum']");
            if (toDelete != null)
                NodesToDelete.Add(toDelete);
            ValuesToReplace.Add("%platTaxNum.label%", string.Empty);
            ValuesToReplace.Add("%platTaxNum.value%", string.Empty);
            
            toDelete = ChangedHtml.DocumentNode.SelectSingleNode("//*[@id='rroFiscalNum']");
            if (toDelete != null)
                NodesToDelete.Add(toDelete);
            ValuesToReplace.Add("%rroFiscalNum.label%", string.Empty);
            ValuesToReplace.Add("%rroFiscalNum.value%", string.Empty);

            return;
        }
    }
}