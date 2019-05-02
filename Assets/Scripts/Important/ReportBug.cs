using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Mail;
using System.Text;
using UnityEngine.UI;
public class ReportBug : MonoBehaviour
{
    InputField Title;
    InputField Description;
    GameObject successText, continueButton, reportBugTitle, sendButton, backButton;
    private void Awake()
    {
        Title = transform.Find("Title").GetComponent<InputField>();
        Description = transform.Find("Description").GetComponent<InputField>();

        successText = transform.Find("Success Text").gameObject;
        continueButton = transform.Find("Continue").gameObject;
        reportBugTitle = transform.Find("Report Bug").gameObject;
        backButton = transform.Find("Back").gameObject;
        sendButton = transform.Find("Send").gameObject;

        successText.SetActive(false);
        continueButton.SetActive(false);

        AsyncTriggers asyncTriggers = FindObjectOfType<AsyncTriggers>();
        continueButton.GetComponent<Button>().onClick.AddListener(asyncTriggers.CloseBugReportCanvas);
        backButton.GetComponent<Button>().onClick.AddListener(asyncTriggers.CloseBugReportCanvas);

    }
    public void sendMessage()
    {
        if (Title.text != "" && Description.text != "")
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("bugreporterforjanda@gmail.com", "BugsSuck123");

            MailMessage mm = new MailMessage("bugreporterforjanda@gmail.com", "bugreporterforjanda@gmail.com", "Bug: " + Title.text, "The Bug is as follows: \n \n" + Description.text);
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm);

            successText.SetActive(true);
            continueButton.SetActive(true);

            Title.gameObject.SetActive(false);
            Description.gameObject.SetActive(false);
            reportBugTitle.SetActive(false);
            sendButton.SetActive(false);
            backButton.SetActive(false);
        }
        else
        {
            if(Title.text == "")
            {
                Text placeholderText = Title.transform.Find("Placeholder").GetComponent<Text>();
                placeholderText.text = "Please fill in...";

            }
            if (Description.text == "")
            {
                Text placeholderText = Description.transform.Find("Placeholder").GetComponent<Text>();
                placeholderText.text = "Please fill in...";

            }
        }
    }
}
