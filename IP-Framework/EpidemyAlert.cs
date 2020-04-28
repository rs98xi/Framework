using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace IP_Framework
{
    class EpidemyAlert : IModule
    {
        private EventHandler fatherHandler;
        private String text = "EpidemyAlert constructor";

        public EpidemyAlert(EventHandler father)
        {
            fatherHandler = father;
            Console.WriteLine(text);
        }

        public override bool InvokeCommand(SubModuleFunctions command, IContext` contextHandler)
        {
            Console.WriteLine("InvokeCommand execution for EpidemyAlert subModule");

            EpidemyContext subModuleContextHandler = contextHandler as EpidemyContext;


            switch (command)
            {
                case SubModuleFunctions.EpidemyCheckForSpecificAlert:
                    
                    DataBaseContext contextSpecific = new DataBaseContext();
                    contextSpecific.databaseId = DataBaseDefines.DatabaseDiseases; // the database id;
                    contextSpecific.databaseFunctionId = DataBaseDefines.DiseasesSpecificQueryDisease; // function ID

                    Dictionary<string, string> dictionarySpecific = new Dictionary<string, string>(); // the params
                    dictionarySpecific.Add("x", subModuleContextHandler.x.ToString());
                    dictionarySpecific.Add("y", subModuleContextHandler.y.ToString());
                    dictionarySpecific.Add("disease", subModuleContextHandler.specificSearch);
                    contextSpecific.ParametersDictionary = dictionarySpecific;

                    EventHandlerContext commandContextSpecific = new EventHandlerContext();
                    commandContextSpecific.command = EventHandlerFunctions.DataBaseModule;
                    commandContextSpecific.subModuleCommand = SubModuleFunctions.DataBaseQueryData;
                    commandContextSpecific.coreCommand = CoreKnownFunctions.InvalidCommand;
                    commandContextSpecific.contextHandler = contextSpecific;

                    bool resultSpecific = fatherHandler.InvokeCommand(commandContextSpecific);

                    if (!resultSpecific)
                    {
                        return false;
                    }

                    foreach (KeyValuePair<string, string> entry in contextSpecific.AnswerDictionary)
                    {
                        // Validate each response

                        subModuleContextHandler.AnswerDictionary[entry.Key] = entry.Value; // perechi boala - zona
                    }

                    if (subModuleContextHandler.AnswerDictionary.Count == 0)
                    {
                        return false;
                    }

                    return true;
                case SubModuleFunctions.EpidemyCheckForAlert:

                    DataBaseContext context = new DataBaseContext();
                    context.databaseId = DataBaseDefines.DatabaseDiseases; // the database id;
                    context.databaseFunctionId = DataBaseDefines.DiseasesFullQuery; // function ID

                    Dictionary<string, string> dictionary = new Dictionary<string, string>(); // the params
                    dictionary.Add("x", subModuleContextHandler.x.ToString());
                    dictionary.Add("y", subModuleContextHandler.y.ToString());
                    context.ParametersDictionary = dictionary;

                    EventHandlerContext commandContext = new EventHandlerContext();
                    commandContext.command = EventHandlerFunctions.DataBaseModule;
                    commandContext.subModuleCommand = SubModuleFunctions.DataBaseQueryData;
                    commandContext.coreCommand = CoreKnownFunctions.InvalidCommand;
                    commandContext.contextHandler = context;

                    bool result = fatherHandler.InvokeCommand(commandContext);

                    if (!result)
                    {
                        return false;
                    }

                    foreach (KeyValuePair<string, string> entry in context.AnswerDictionary)
                    {
                        // Validate each response

                        subModuleContextHandler.AnswerDictionary[entry.Key] = entry.Value; // perechi boala - zona
                    }

                    if (subModuleContextHandler.AnswerDictionary.Count == 0)
                    {
                        return false;
                    }

                    return true;
                default:
                    return false;
            }
        }

        public override bool Init(byte[] context, int sizeOfContext)
        {
            Console.WriteLine("Init execution");
            return true;
        }

        public override bool UnInit()
        {
            Console.WriteLine("UnInit execution");
            return true;
        }
    }
}
