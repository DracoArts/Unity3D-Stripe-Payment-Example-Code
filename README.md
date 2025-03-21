
# Welcome to DracoArts
![Logo](https://dracoarts-logo.s3.eu-north-1.amazonaws.com/DracoArts.png)



# Stripe integration Unity 

Stripe is a leading financial technology company that provides payment processing infrastructure for businesses of all sizes. It enables businesses to accept payments online, in-person, and via mobile devices. Stripe is widely used for its developer-friendly APIs, robust security, and extensive feature set.

# Key Features of Stripe
  ## 1.Payment Processing:

Accept credit/debit cards, digital wallets (Apple Pay, Google Pay), and other payment methods.

Support for over 135 currencies and various payment options (ACH, SEPA, etc.).

 ## 2.Subscription Billing:

Handle recurring payments for subscriptions and memberships.

Automate invoicing, prorations, and trial periods.

 ## 3.Global Reach:

Operates in over 46 countries.

Supports multi-currency payments and automatic currency conversion.

## 4.Developer-Friendly APIs:

Easy-to-integrate RESTful APIs and SDKs for various programming languages.

Prebuilt UI components like Stripe Elements for seamless payment forms.

 ## 5.Security and Compliance:

PCI-DSS Level 1 compliance (highest level of security for payment processing).

Built-in fraud prevention tools like Stripe Radar.

## 6.Customizable Checkout:

Hosted payment pages (Stripe Checkout) for quick integration.

Customizable payment flows for a branded experience.

 ## 7.Webhooks:

Real-time notifications for payment events (e.g., successful payments, failed charges, subscription updates).

## 8.Reporting and Analytics:

Detailed dashboards for tracking payments, revenue, and customer activity.

Exportable reports for accounting and analysis.

# Set Up a Stripe Test Account
Sign up for a Stripe account at [Stripe](https://stripe.com/) .

Switch to Test mode in the Stripe Dashboard.

Get your test API keys:

## Publishable key
 (pk_test_...) for the client-side (Unity).

## Secret key 
(sk_test_...) for the server-side (backend).



## Usage/Examples
     // Replace with your Stripe keys
    private const string PrivateKey = "sk_test_51R1kPCGxtmqQP5vA9ELpji0ytMzF3119MGnKbFNrtcemG3GEsVZdA3BIgWqbkAFb9zfLYQbl8CtAAQ2sdoadz9YD00OnGCmqfI";
    private const string PublishableKey = "pk_test_51R1kPCGxtmqQP5vAGDGhFhf7Ff6gXMpaBtLWc01MF9xOatgGefyQirwLJPWWDqGgTRpzXi2Y9YUI1d9p5vgQYOpA00UdKFRSS9";


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

    private void CreateCreditCardToken(string cardNumber, int expMonth, int expYear, string cvc){
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

## Images
![Logo](https://raw.githubusercontent.com/AzharKhemta/DemoClient/refs/heads/main/Stripe%20payment.gif)
## Authors

- [@MirHamzaHasan](https://github.com/MirHamzaHasan)
- [@WebSite](https://mirhamzahasan.com)

 - 


## 🔗 Links

[![linkedin](https://img.shields.io/badge/linkedin-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/company/mir-hamza-hasan/posts/?feedView=all/)
## Tech Stack
**Client:** Unity,C#

**Server:** Stripe 


## Documentation

[Documentation](https://docs.stripe.com/)

