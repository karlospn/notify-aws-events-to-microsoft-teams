import json
import os
import logging
import urllib.request
from urllib.error import URLError, HTTPError

logger = logging.getLogger()
logger.setLevel(logging.DEBUG)

def lambda_handler(event, context):   
    try:       
        data = json.dumps(event)
        msg = {
            'text': data
        }    
        response = urllib.request.urlopen(urllib.request.Request(os.environ['teams_webhook_uri'], json.dumps(msg).encode('utf-8')))
        response.read()
    except HTTPError as err:
        logger.error(f"Request failed: {err.code} {err.reason}")
    except URLError as err:
        logger.error(f"Server connection failed: {err.reason}")
