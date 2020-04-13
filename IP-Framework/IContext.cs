using System;
using System.Collections.Generic;
using System.Text;

namespace IP_Framework
{
    class IContext
    {
        public byte[] context;
        public int sizeOfContext;

        public byte[] answer;
        public int sizeOfAnswer;

        public IContext(byte[] initContext, int initSizeOfContext)
        {
            context = initContext;
            sizeOfContext = initSizeOfContext;

            string raspuns = "Not implemented yet, but here should stay the answer for the query";
            sizeOfAnswer = raspuns.Length;
            answer = Encoding.ASCII.GetBytes(raspuns);
        }

        public IContext()
        {
            context = null;
            sizeOfContext = 0;

            string raspuns = "Not implemented yet, but here should stay the answer for the query";
            sizeOfAnswer = raspuns.Length;
            answer = Encoding.ASCII.GetBytes(raspuns);
        }
    }

    class ImageContext : IContext
    {
        // to be discussed and implemented
    }

    class EpidemyContext : IContext
    {
        // to be discussed and implemented
    }

    class SymptomLearningContext : IContext
    {
        // to be discussed and implemented
    }

    class FormContext : IContext
    {
        // to be discussed and implemented
    }

    class DataBaseContext : IContext
    {
        // to be discussed and implemented
    }
}
