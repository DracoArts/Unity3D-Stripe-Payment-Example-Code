using TMPro;
using System;
using Stripe;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StripeManager : MonoBehaviour
{
    // Replace with your Stripe keys
    private const string PrivateKey = "sk_test_51R1kPCGxtmqQP5vA9ELpji0ytMzF3119MGnKbFNrtcemG3GEsVZdA3BIgWqbkAFb9zfLYQbl8CtAAQ2sdoadz9YD00OnGCmqfI";
    private const string PublishableKey = "pk_test_51R1kPCGxtmqQP5vAGDGhFhf7Ff6gXMpaBtLWc01MF9xOatgGefyQirwLJPWWDqGgTRpzXi2Y9YUI1d9p5vgQYOpA00UdKFRSS9";

    [Header("Input Fields")]
    [SerializeField] private InputField inputCardNumber;
    [SerializeField] private InputField inputExpMonth;
    [SerializeField] private InputField inputExpYear;
    [SerializeField] private InputField inputCvc;
    [SerializeField] private InputField email;


    [Header("UI Elements")]
    [SerializeField] private Transform cardPanel;
    [SerializeField] private Transform errorPanel;
    [SerializeField] private Transform successPanel;
    [SerializeField] private Text textError;
    [SerializeField] private Button payButton;
    [SerializeField] private GameObject loader;

    private string transactionId;
    private string transactionUrl;
    private string amount;

    private void Start()
    {


        successPanel.gameObject.SetActive(false);
        errorPanel.gameObject.SetActive(false);

        payButton.onClick.AddListener(() =>
        {
            loader.SetActive(true);
            GetToken();
        });

    }

    public void GetToken()
    {
        StartCoroutine(DelayGetToken());
    }

    private IEnumerator DelayGetToken()
    {
        yield return new WaitUntil(() => loader.activeInHierarchy);

        string cardNumber = inputCardNumber.text;
        string cvc = inputCvc.text;

        if (!int.TryParse(inputExpMonth.text, out int expMonth) || expMonth < 1 || expMonth > 12)
        {
            ShowError("Invalid Expiration Month");
            yield break;
        }

        if (!int.TryParse(inputExpYear.text, out int expYear) || expYear < DateTime.Now.Year)
        {
            ShowError("Invalid Expiration Year");
            yield break;
        }

        yield return new WaitForSeconds(1f);

        CreateCreditCardToken(cardNumber, expMonth, expYear, cvc);

    }

 private void CreateCreditCardToken(string cardNumber, int expMonth, int expYear, string cvc)
    {
        try
        {
            StripeConfiguration.ApiKey = PublishableKey;
            var options = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Number = cardNumber,
                    ExpMonth = expMonth.ToString(),
                    ExpYear = expYear.ToString(),
                    Cvc = cvc
                }
            };

            var service = new TokenService();
            Token stripeToken = service.Create(options);
        
            Debug.Log("Stripe Token ID: " + stripeToken.Id);
            CustomerUpdate();
            ProcessPayment(stripeToken.Id);
        }
    catch (Exception e)
    {
        Debug.LogError(e);
        ShowError(e.Message);
    }
}



    private void CustomerUpdate()
    {
        try
        {
            StripeConfiguration.ApiKey = PrivateKey;

            var options = new CustomerListOptions
            {
                Email = email.text,
                Limit = 1,
            };

            var service = new CustomerService();
              StripeList<Customer> customers = service.List(options);

            if (customers.Any())
            {
                Debug.Log("Customer already exists.");
            }
            else
            {
               var options1 = new CustomerCreateOptions
                {
                    Email = email.text,

                };
                CustomerService service1 = new CustomerService();
                Customer newCustomer = service1.Create(options1);
                Debug.Log(newCustomer.Email);
            }
        }
        catch (Exception e)
        {
            Debug.Log("Customer update error: " + e.Message);
        }
    }

    private void ProcessPayment(string token)
    {
        try
        {
            StripeConfiguration.ApiKey = PrivateKey;

            var options = new ChargeCreateOptions
            {
                Amount = 200 * 100, // Amount in cents
                Currency = "usd",
                Source = token,
                Description = "Payment Charge for Your Product",
                
            };

            var service = new ChargeService();
            var charge = service.Create(options);

            transactionId = charge.Id;
            transactionUrl = charge.ReceiptUrl;
            amount = charge.Amount.ToString();

            successPanel.gameObject.SetActive(true);
            cardPanel.gameObject.SetActive(false);
            loader.SetActive(false);

            Debug.Log($"Payment successful! Transaction ID: {transactionId}");
        }
        catch (StripeException ex)
        {
            ShowError("Error processing payment: " + ex.Message);
        }
    }

    private void ShowError(string message)
    {
        Debug.LogError(message);
        textError.text = message;
        errorPanel.gameObject.SetActive(true);
        loader.SetActive(false);
    }
}
