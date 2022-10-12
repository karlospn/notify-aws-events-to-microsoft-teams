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
             _ = new EventBridgeEcrPushRuleConstruct(this,
                "ecr-push-rule-notifier");

             _ = new EventBridges3BucketRuleConstruct(this,
                 "s3-create-or-delete-bucket-notifier");

        }
    }
}
