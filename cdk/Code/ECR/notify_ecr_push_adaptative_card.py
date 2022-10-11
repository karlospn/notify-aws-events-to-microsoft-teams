import json
import os
import logging
import urllib.request
from urllib.error import URLError, HTTPError

logger = logging.getLogger()
logger.setLevel(logging.DEBUG)

def lambda_handler(event, context):   
    try:
        msg = {
                "type": "message",
                "attachments": [
                    {
                        "contentType": "application/vnd.microsoft.card.adaptive",
                        "content": {
                            "type": "AdaptiveCard",
                            "body": [
                            {
                                "type": "TextBlock",
                                "size": "High",
                                "weight": "Bolder",
                                "text": "New ECR image available"
                            },
                            {
                                "type": "FactSet",
                                "facts": [
                                    {
                                        "title": "Image Name:",
                                        "value": f"{event['detail']['repository-name']}"
                                    },
                                    {
                                        "title": "Version Tag:",
                                        "value": f"{event['detail']['image-tag']}"
                                    }
                                ]
                            }
                            ],
                            "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
                            "version": "1.0",
                            "msteams": {
                            "width": "Full"
                            }
                        }
                    }
                ]
            }
        
        response = urllib.request.urlopen(urllib.request.Request(os.environ['teams_webhook_uri'], json.dumps(msg).encode('utf-8')))
        response.read()
    except HTTPError as err:
        logger.info(err)
        logger.error(f"Request failed: {err.code} {err.reason}")
    except URLError as err:
        logger.error(f"Server connection failed: {err.reason}")
