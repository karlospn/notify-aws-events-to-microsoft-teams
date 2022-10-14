using Amazon.CDK;
using Constructs;
using EventBridgeCdkStack.Constructs;

namespace EventBridgeCdkStack.Stacks
{
    public class CdkStack : Stack
    {
        internal CdkStack(Construct scope,
            string id,
            IStackProps props = null)
            : base(scope, id, props)
        {
            var teamsWebHookUri = new CfnParameter(this, 
                "teamsWebHookUri", 
                new CfnParameterProps
            {
                Type = "String",
                Description = "The URI of the Microsoft Teams Webhook"
            });

            _ = new EventBridgeEcrPushRuleConstruct(this,
                "ecr-push-rule-notifier", 
                teamsWebHookUri.ValueAsString);

             _ = new EventBridges3BucketRuleConstruct(this,
                 "s3-create-or-delete-bucket-notifier",
                 teamsWebHookUri.ValueAsString);

        }
    }
}
