using System;
using System.Collections.Generic;
using System.Text;

namespace IP_Framework
{

    class EventHandlerContext  // the context needed for the invoke function
    {
        public EventHandlerFunctions command;
        public IContext contextHandler;
        public CoreKnownFunctions coreCommand;
        public SubModuleFunctions subModuleCommand;

        public EventHandlerContext()
        {
            command = EventHandlerFunctions.InvalidCommand;
            coreCommand = CoreKnownFunctions.InvalidCommand;
            subModuleCommand = SubModuleFunctions.InvalidCommand;
            contextHandler = null;
        }

        public EventHandlerContext(byte[] initContext, int initSizeOfContext) // this will be changed with an interface for contexts
        {
            command = EventHandlerFunctions.InvalidCommand;
            coreCommand = CoreKnownFunctions.InvalidCommand;
            subModuleCommand = SubModuleFunctions.InvalidCommand;
            contextHandler = new IContext(initContext, initSizeOfContext);
        }

    }

    class EventHandler
    {
        private SymptomBasedDetection symptomBasedModule;
        private EpidemyAlert epidemyModule;
        private ImageProcessing imageModule;
        private SymptomLearning symptomModule;
        private DataBaseHandler dbHandler; // TODO after the DB is alive
        // all modules should pe private, we need to encapsulate as much as possible
        // only this instances should have access to the data in the handler

        public EventHandler() // this module should get an DB instance for the dataBaseHandler
        {
            Init(null, 0);
            Console.WriteLine("A mers initilizarea!");
        }

        ~EventHandler()
        {
            this.UnInit(); // this should be like this because we need the same code for forced de-initialization
            Console.WriteLine("A mers de-initilizarea!");

        }

        public bool InvokeCommand(EventHandlerContext handlerContext) // invoke commands between the submodules and also the Core
        {
            Console.WriteLine("InvokeCommand execution for subModule Handler");

            if (ValidateContext(handlerContext) == false)
                return false;
            switch (handlerContext.command)
            {
                case EventHandlerFunctions.Init:
                    return this.Init(handlerContext.contextHandler.context, handlerContext.contextHandler.sizeOfContext);
                case EventHandlerFunctions.UnInit:
                    return UnInit();
                case EventHandlerFunctions.RequestCommand:
                    return RequestCommand(handlerContext.coreCommand, handlerContext.contextHandler.context, handlerContext.contextHandler.sizeOfContext);
                case EventHandlerFunctions.EpidemyAlertModule:
                    return epidemyModule.InvokeCommand(handlerContext.subModuleCommand, handlerContext.contextHandler);
                case EventHandlerFunctions.ImageProcessingModule:
                    return imageModule.InvokeCommand(handlerContext.subModuleCommand, handlerContext.contextHandler);
                case EventHandlerFunctions.SymptomLearningModule:
                    return symptomModule.InvokeCommand(handlerContext.subModuleCommand, handlerContext.contextHandler);
                case EventHandlerFunctions.SymptomBasedDetectionModule:
                    return symptomBasedModule.InvokeCommand(handlerContext.subModuleCommand, handlerContext.contextHandler);
                case EventHandlerFunctions.DataBaseModule:
                    return dbHandler.InvokeCommand(handlerContext.subModuleCommand, handlerContext.contextHandler);
                case EventHandlerFunctions.InvokeCommand:
                    return false; // INvoKE COmMAnD
            }
            return false;
        }

        private bool RequestCommand(CoreKnownFunctions command, byte[] context, int sizeOfContext)  // Sends requests to Core and then process them as we want
        {
            // invoke commands directly from the core of the program itself
            // directly send command to core, where the context should be validated!!!!
            // we could return actually a command, not just asking, but its better like this because we can control the state of the module in case of malfunctioning after our call
            // also, if we return the command, we can't use the context asked
            return true;
        }

        private bool Init(byte[] context, int sizeOfContext)  // Initialize the data using the possible context, it should be checked if context is not null if its mandatory for Init
        {
            symptomBasedModule = new SymptomBasedDetection(this);
            symptomBasedModule.Init(context, sizeOfContext);
            epidemyModule = new EpidemyAlert(this);
            epidemyModule.Init(context, sizeOfContext);
            imageModule = new ImageProcessing(this);
            imageModule.Init(context, sizeOfContext);
            symptomModule = new SymptomLearning(this);
            symptomModule.Init(context, sizeOfContext);
            dbHandler = new DataBaseHandler(this);
            dbHandler.Init(context, sizeOfContext);
            return true;
        }

        private bool UnInit()  // unInit all the modules and destroy all data left in memory
        {
            symptomBasedModule.UnInit();
            epidemyModule.UnInit();
            imageModule.UnInit();
            symptomModule.UnInit();
            dbHandler.UnInit();
            return true;
        }

        private bool ValidateContext(EventHandlerContext contextContainer)
        {
            if (ReferenceEquals(contextContainer, null))  // useless until EventHandlerContext becomes a class, not a struct
                return false;
            if (contextContainer.contextHandler.sizeOfContext == 0)
                return false;
            if (ReferenceEquals(contextContainer.contextHandler.context, null))
                return false;
            return true;
        }
    }

    public enum EventHandlerFunctions // all the things the Handler should/can do
    {
        InvalidCommand = 0,

        Init = 1,
        InvokeCommand,
        RequestCommand,
        UnInit,

        SymptomBasedDetectionModule = 100,
        EpidemyAlertModule,
        ImageProcessingModule,
        SymptomLearningModule,
        DataBaseModule,
    }

    public enum CoreKnownFunctions // this should be placed in Core, for everybody to add complexity needed
    {
        InvalidCommand = 0,

        DiagnosisUnInit = 1,
        DiagnosisRestart,
        DiagnosisTriggerDiagnostic,
        DiagnosisInvoke,

    }

    public enum SubModuleFunctions // all the functions for all the modules, should stay categorized!
    {
        InvalidCommand = 0,

        MachineLearningAsk = 1,
        MachineLearningGetResults,
        MachineLearningStoreResults,
        MachineLearningAdapt,

        CreateForm = 101,
        AskForFormResults,
        SaveFormResults,
        StartForm,

        ImageAddPhoto = 201,
        ImageComparePhoto,
        ImageStoreResults,
        ImageAdapt,

        EpidemyCheckForAlert = 301,
        EpidemyCheckForSpecificAlert,

        DataBaseSaveData = 401,
        DataBaseDestroyData,
        DataBaseQueryData,
        DataBaseAlterData,
    }

    public enum DataBaseDefines
    {
        DatabaseDiseases = 0,
        DiseasesFullQuery,
        DiseasesSpecificQueryDisease,
        DiseasesSpecificQueryPerson,
    }
}
