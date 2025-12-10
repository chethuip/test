/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Types.Authentication;

namespace OXPd2ExamplesHost.Services
{
    public interface IAuthenticationAgentService
    {
        PostPromptResult GetPostPromptResult();
        void SetPostPromptResult(PostPromptResult result);
    }

    public class AuthenticationAgentService : IAuthenticationAgentService
    {
        #region Construction and singleton stuff
        private PostPromptResult postPromptResult;

        public AuthenticationAgentService()
        {
            postPromptResult = new PostPromptResult();
        }

        #endregion // Construction
        
        public PostPromptResult GetPostPromptResult()
        {
            return postPromptResult;
        }

        public void SetPostPromptResult(PostPromptResult result)
        {
            postPromptResult = result;
        }
    }
}
