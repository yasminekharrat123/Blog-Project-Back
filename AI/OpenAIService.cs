using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AI {
public interface IOpenAIService
{
     string Evaluate(Blog.Models.Blog blog);
    // JObject Enhance(Blog.Models.Blog blog);
}
public class OpenAIService: IOpenAIService 
{
    public string  Evaluate(Blog.Models.Blog blog) 
    {
        var formattingFunctionSpecs = new
        {
            name = "format_evaluation_response",
            description = "Extracts the blog evaluation into a structured json",
            parameters = new
            {
                type = "object",
                properties = new
                {
                    Clarity = new
                    {
                        type = "object",
                        properties = new
                        {
                            rating = new
                            {
                                type = "integer",
                                description = "the Clarity and Structure rating",
                                minimum = 0,
                                maximum = 10,
                            },
                            evaluation = new
                            {
                                type = "string",
                                description = "the Clarity and Structure evaluation",
                            },
                        },
                    },
                    Writing = new
                    {
                        type = "object",
                        properties = new
                        {
                            rating = new
                            {
                                type = "integer",
                                description = "the rating on Writing Style",
                                minimum = 0,
                                maximum = 10,
                            },
                            evaluation = new
                            {
                                type = "string",
                                description = "the Writing Style evaluation",
                            },
                        },
                    },
                    Accuracy = new
                    {
                        type = "object",
                        properties = new
                        {
                            rating = new
                            {
                                type = "integer",
                                description = "the rating on the Accuracy of Information",
                                minimum = 0,
                                maximum = 10,
                            },
                            evaluation = new
                            {
                                type = "string",
                                description = "the Accuracy of Information evaluation",
                            },
                        },
                    },
                    Persuasiveness = new
                    {
                        type = "object",
                        properties = new
                        {
                            rating = new
                            {
                                type = "integer",
                                description = "the rating on Persuasiveness",
                                minimum = 0,
                                maximum = 10,
                            },
                            evaluation = new
                            {
                                type = "string",
                                description = "the Persuasiveness evaluation",
                            },
                        },
                    }
                },
            },
        };

        var payload = new
        {
            model = "gpt-3.5-turbo",
            n = 1,
            temperature = 0,
            messages = new dynamic[]
            {
                new
                {
                    role = "system",
                    content = "Use `format_evaluation_response` function instead of adding yourself.",
                },
                new
                {
                    role = "user",
                    content = 
                            $"Pretend you're a literary expert. Consider this blog titled: {blog.Title} \n , {blog.Body} \n"+
                            "I'm seeking an evaluation of the blog post's overall quality, coherence, and persuasiveness. Specifically, I'd like feedback on the following aspects:"+

                            "1. Clarity and Structure: Does the blog post have a clear and logical structure? Are the main points well-organized and easy to follow? elaborate in 3 to 4 lines and give a rating out of 10"+

                            "2. Writing Style: How engaging and concise is the writing style? Does it effectively convey the information and maintain the reader's interest? elaborate  in 3 to 4 lines and give a rating out of 10"+

                            "3. Accuracy of Information: Are the facts and statistics presented accurate and up-to-date? Are there any inaccuracies or inconsistencies? elaborate  in 3 to 4 lines and give a rating out of 10"+

                            "4. Persuasiveness: Does the blog post effectively persuade the reader of the importance of renewable energy for the environment? Are the arguments well-supported? elaborate  in 3 to 4 lines and give a rating out of 10"+

                            "Please firmly evaluate the blog post based on the above criteria and provide constructive feedback. Your insights will help me enhance the quality of this content before publishing it. Thank you!",
                },
            },
            functions = new dynamic[] { formattingFunctionSpecs },
        };

        var payloadJson = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
        return payloadJson;

    }
}



}