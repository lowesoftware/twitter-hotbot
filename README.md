**Purpose**

I was inspired to write this twitter bot after learning years ago that Google would offer support numbers for people who would search for suicide related terms. I wanted to do something similar for Twitter to put a little positiveness back into the global conversation. In practice I ran this application for several days or weeks and looked for tweets from people who were sad or depressed. The bot would then respond with something uplifiting or inspirational.

**How It Works**

This is a .Net application written in C#. It uses a couple off the shelf text classification libraries in order to score/classify tweets.

This application is a WinForms GUI, several support libraries, and backed by a MSSQL database (accessed by Entity Framework).

The logic of this application is divided into jobs -- a job is composed of:

1. A list of Twitter accounts used to distribute the jobs activities.
2. A list of search terms used to search for tweets -- this is to do an initial coarse filter.
3. A training session to classify tweets.
4. A set of canned responses to send back to users if their message.

The basic usage pattern of this application is to configure the job parameters, manually retrieve tweets and classify them in order to train the bot, and then hit the go button in order for it to regularly search for tweets and respond to them.