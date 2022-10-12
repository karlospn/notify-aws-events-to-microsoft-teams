using System.Collections.Generic;
using Amazon.CDK.AWS.Events;
using Amazon.CDK.AWS.Events.Targets;
using Amazon.CDK.AWS.Lambda;
using Constructs;
using EventBus = Amazon.CDK.AWS.Events.EventBus;

namespace EventBridgeCdkStack.Constructs
{
    public class EventBridges3BucketRuleConstruct : Construct
    {
        public EventBridges3BucketRuleConstruct(Construct scope, 
            string id)
            : base(scope, id)
        {

            var bus = EventBus.FromEventBusName(this,
                "default-event-bus",
                "default");

            var rule = new Rule(this, 
                "rule-notify-s3-bucket-create-or-delete", 
                new RuleProps
            {
                EventBus = bus,
                Description = "Sent a teams notification via lambda when an s3 bucket is created or deleted.",
                RuleName = "rule-s3-bucket-create-or-delete-teams-notification",
                Enabled = true,
                EventPattern = new EventPattern
                {
                    Source = new []{"aws.s3"},
                    DetailType = new []{ "AWS API Call via CloudTrail" },
                    Detail = new Dictionary<string, object>
                    {
                        {"eventSource", new[]{ "s3.amazonaws.com" } },
                        {"eventName", new[]{ "DeleteBucket", "CreateBucket" } },
                    }
                }
            });

            var fn = new Function(this, 
                "lambda-s3-create-or-delete-notification-teams",
                new FunctionProps
            {
                Runtime = Runtime.PYTHON_3_9,
                Handler = "notify_s3_bucket_creation_or_deletion_adaptative_card.lambda_handler",
                Code = Code.FromAsset("Code/S3"),
                Environment = new Dictionary<string, string>
                {
                    {"teams_webhook_uri", "add-teams-webhook-uri"}
                }
            });

            rule.AddTarget(new LambdaFunction(fn));

        }
    }
}
