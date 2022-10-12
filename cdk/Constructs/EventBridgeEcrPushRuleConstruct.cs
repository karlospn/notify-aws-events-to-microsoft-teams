using System.Collections.Generic;
using Amazon.CDK.AWS.Events;
using Amazon.CDK.AWS.Events.Targets;
using Amazon.CDK.AWS.Lambda;
using Constructs;
using EventBus = Amazon.CDK.AWS.Events.EventBus;

namespace EventBridgeCdkStack.Constructs
{
    public class EventBridgeEcrPushRuleConstruct : Construct
    {
        public EventBridgeEcrPushRuleConstruct(Construct scope, 
            string id,
            string teamsWebHookUri)
            : base(scope, id)
        {

            var bus = EventBus.FromEventBusName(this,
                "default-event-bus",
                "default");

            var rule = new Rule(this, 
                "rule-notify-ecr-push-img", 
                new RuleProps
            {
                EventBus = bus,
                Description = "Sent a teams notification via lambda when an ECR push event is generated.",
                RuleName = "rule-ecr-push-image-teams-notification",
                Enabled = true,
                EventPattern = new EventPattern
                {
                    Source = new []{"aws.ecr"},
                    DetailType = new []{"ECR Image Action"},
                    Detail = new Dictionary<string, object>
                    {
                        {"action-type", new[]{"PUSH"} },
                        {"result", new[]{"SUCCESS"} },
                    }
                }
            });

            var fn = new Function(this, 
                "lambda-ecr-push-image-notification-teams",
                new FunctionProps
            {
                Runtime = Runtime.PYTHON_3_9,
                Handler = "notify_ecr_push_adaptative_card.lambda_handler",
                Code = Code.FromAsset("Code/ECR"),
                Environment = new Dictionary<string, string>
                {
                    {"teams_webhook_uri", $"{teamsWebHookUri}"}
                }
            });

            rule.AddTarget(new LambdaFunction(fn));

        }
    }
}
