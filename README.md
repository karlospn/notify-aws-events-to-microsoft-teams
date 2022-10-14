# How to notify AWS Events to Microsoft Teams using AWS EventBridge and AWS Lambda

## **Introduction**

An AWS event indicates a change in an environment, a SaaS partner service or application, or one of your applications or services. The following are examples of AWS events:

- Amazon EC2 generates an event when the state of an instance changes from pending to running.
- Amazon EC2 Auto Scaling generates events when it launches or terminates instances.
- AWS EC2 generates events when you push a new image into an ECR Repository.

Here's the full list of AWS services that generate events:
- https://docs.aws.amazon.com/eventbridge/latest/userguide/eb-service-event.html 

Events are represented as JSON objects and they all have a similar structure.

This repository has an ``AWS CDK`` app that contains a couple of examples of how to notify those AWS events to a Microsoft Teams channel using AWS EventBridge and AWS Lambda.

## **Content**

The setup to notify an AWS Event to Microsoft Teams is really simple:

![notify-aws-event-to-teams-diagram](https://raw.githubusercontent.com/karlospn/notify-aws-events-to-microsoft-teams/main/docs/notify-aws-event-to-teams.png)

- An AWS EventBridge Rule listens for a specific set of AWS events and executes a Lambda function when any of those concrete events occurs.
- The Lambda function sends the event to a Microsoft Teams Channel using an incoming HTTP WebHook.

## **CDK App Resources**

The CDK app creates **2 EventBridge Rules** and **2 lambda functions**:
- The first rule notifies when a new container image or tag gets pushed into ``ECR`` and a lambda function post this event to Teams.
- The second rule notifies when an ``S3 Bucket`` is created or deleted and another lambda function post those events to Teams.


## **How to deploy the CDK app**

1. Create an "Incoming Webhook" on one of your Microsoft Teams Channels.

![teams-incoming-webhook](https://raw.githubusercontent.com/karlospn/notify-aws-events-to-microsoft-teams/main/docs/teams-incoming-webhook.png)

1. Deploy the CDK app.

To deploy it, use the command:   
- ``cdk deploy --profile <profile> --parameters teamsWebHookUri=<incoming-teams-webhook-uri>``

Or the command:   
- ``cdk deploy --parameters teamsWebHookUri=<incoming-teams-webhook-uri>``

The ``CDK`` app uses the ``CDK_DEFAULT_ACCOUNT`` and ``CDK_DEFAULT_REGION`` environment variables to specify the account and the region where the infrastructure will be created.   

If you hard-code the target account and region on your ``CDK`` app, the stack will always be deployed to that specific account and region.   
To make the stack deployable to a different target, but to determine the target at synthesis time, your stack can use two environment variables provided by the ``AWS CDK CLI``: ``CDK_DEFAULT_ACCOUNT`` and ``CDK_DEFAULT_REGION``. These variables are set based on the AWS profile specified using the ``--profile`` option, or the ``default AWS profile`` if you don't specify one.

Here's an example of how to deploy the app:
- ``cdk deploy --parameters teamsWebHookUri=https://cponsn.webhook.office.com/webhookb2/845c5df3-e285-4e3b-8a57-35a5543a05da@532ddc14-1479-45c7-b836-efbccb2bf6aa/IncomingWebhook/45a42011bfe54a2091567af10968422
2/a1d89e88-1b21-4da6-a2b1-dfb848d8b956``

## **How to test it**

- Create an ``S3 bucket``, and take a look at your Teams Channel.

![s3-bucket-create-adaptative-card](https://raw.githubusercontent.com/karlospn/notify-aws-events-to-microsoft-teams/main/docs/s3-bucket-create-adaptative-card.png)

- Delete an ``S3 bucket``, and take a look at your Teams Channel.

![s3-bucket-delete-adaptative-card](https://raw.githubusercontent.com/karlospn/notify-aws-events-to-microsoft-teams/main/docs/s3-bucket-delete-adaptative-card.png)

- Push a new image into an ``ECR repository``, and take a look at your Teams Channel.

![ecr-push-img](https://raw.githubusercontent.com/karlospn/notify-aws-events-to-microsoft-teams/main/docs/teams-adaptative-card.png)