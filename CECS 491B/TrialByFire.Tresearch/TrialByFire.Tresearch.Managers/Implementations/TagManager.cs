using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    /// <summary>
    ///     Manages the creation and deletion of tags from word bank as well as adding and removing tags from nodes. Handles delegation of services.
    /// </summary>
    public class TagManager : ITagManager
    {
        private IAccountVerificationService _accountVerificationService { get; set; }       //Use to verify account exists, enabled and confirmed. Checks if account is authorized to make changes 
        private IMessageBank _messageBank { get; set; }                                     //Used to send status codes
        private ITagService _tagService { get; set; }                                       //Performs business logic
        private BuildSettingsOptions _options { get; }                                      //Holds webapi key and connection string
        public TagManager(IAccountVerificationService accountVerificationService, IMessageBank messagebank, ITagService tagService, IOptions<BuildSettingsOptions> options)
        {
            _accountVerificationService = accountVerificationService; 
            _messageBank = messagebank;
            _tagService = tagService;
            _options = options.Value;
        }

        /// <summary>
        ///     Adds a tag to a list of nodes. UserAccount is checked if valid and is authorized to make changes to nodes.
        /// </summary>
        /// <param name="nodeIDs">List of nodes' id(s)</param>
        /// <param name="tagName">String tag to add to nodes</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status code</returns>
        public async Task<string> AddTagToNodesAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Check if user is authenticated
                if (Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated).ConfigureAwait(false);
                }

                // Check if tag name is null, empty string or all space
                if (tagName == null || tagName.Equals("") || tagName.Trim().Equals(""))
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagNameInvalid).ConfigureAwait(false);
                }
                    
                // Check if node list is null or empty
                if (nodeIDs == null || nodeIDs.Count() <= 0)
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound).ConfigureAwait(false);
                }

                // Get user's role
                string role = "";
                if (Thread.CurrentPrincipal.IsInRole(_options.User))
                {
                    role = _options.User;
                }
                else if (Thread.CurrentPrincipal.IsInRole(_options.Admin))
                {
                    role = _options.Admin;
                }
                else
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.unknownRole).ConfigureAwait(false);
                }
                    
                string userHash = (Thread.CurrentPrincipal.Identity as IRoleIdentity).UserHash;

                IAccount account = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                // Verify if account is enabled and confirmed
                string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(account, cancellationToken).ConfigureAwait(false);

                // Check if account is enabled and confirme, if not return error
                if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess).ConfigureAwait(false)))
                {
                    return resultVerifyAccount;
                }

                // Verify if account is authorized to make changes to Nodes
                string resultVerifyAuthorized = await _accountVerificationService.VerifyAccountAuthorizedNodeChangesAsync(nodeIDs, userHash, cancellationToken).ConfigureAwait(false);

                // Check status if account is authorized to make changes
                if (!resultVerifyAuthorized.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess).ConfigureAwait(false)))
                {
                    return resultVerifyAuthorized;
                } 

                string result = await _tagService.AddTagToNodesAsync(nodeIDs, tagName, cancellationToken).ConfigureAwait(false);

                return result;
            }
            catch (OperationCanceledException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }

        /// <summary>
        ///     RemoveTagFromNodes(nodeIDs, tagName)
        ///         Removes a tag from a list of nodes. UserAccount is checked if valid and is authorized to make changes to nodes.
        /// </summary>
        /// <param name="nodeIDs">List of node ids to add tag </param>
        /// <param name="tagName">String tag name</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status code</returns>
        public async Task<string> RemoveTagFromNodesAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {

                cancellationToken.ThrowIfCancellationRequested();

                // Check if the user is authenticated
                if (Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated).ConfigureAwait(false);
                }
                    

                // Check if tag name is null, empty string or all space
                if (tagName == null || tagName.Equals("") || tagName.Trim().Equals(""))
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagNameInvalid).ConfigureAwait(false);
                }
                    
                // Check if node list is null or empty
                if (nodeIDs == null || nodeIDs.Count() <= 0)
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound).ConfigureAwait(false);
                }
                    

                // Get user's role
                string role = "";
                if (Thread.CurrentPrincipal.IsInRole(_options.User))
                {
                    role = _options.User;
                }
                else if (Thread.CurrentPrincipal.IsInRole(_options.Admin))
                {
                    role = _options.Admin;
                }
                else
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.unknownRole).ConfigureAwait(false);
                }

                string userHash = (Thread.CurrentPrincipal.Identity as IRoleIdentity).UserHash;

                // Create UserAccount with user's username and role
                IAccount account = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                // Verify if account is enabled and confirmed
                string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(account, cancellationToken).ConfigureAwait(false);

                // Check status of account verification, if not return error
                if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess)))
                    return resultVerifyAccount;

                // Verify if account is authorized to make changes to Nodes
                string resultVerifyAuthorized = await _accountVerificationService.VerifyAccountAuthorizedNodeChangesAsync(nodeIDs, userHash, cancellationToken).ConfigureAwait(false);

                // Check status of authorization verification, if not return error
                if (!resultVerifyAuthorized.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess).ConfigureAwait(false)))
                    return resultVerifyAuthorized;

                // Remove tag from node(s)
                string result = await _tagService.RemoveTagFromNodesAsync(nodeIDs, tagName, cancellationToken).ConfigureAwait(false);

                return result;
            }
            catch (OperationCanceledException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }
    
        /// <summary>
        ///     GetNodeTagAsync(nodeIDs, 
        /// </summary>
        /// <param name="nodeIDs"></param>
        /// <param name="order"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of shared tags and string result</returns>
        public async Task<Tuple<List<string>, string>> GetNodeTagsAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Check if the user is authenticated
                if (Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    return Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated).ConfigureAwait(false));
                }

                // Check if node list is null or empty
                if (nodeIDs == null || nodeIDs.Count() <= 0)
                {
                    return Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound).ConfigureAwait(false));
                }
                    
                // Get user's role
                string role = "";
                if (Thread.CurrentPrincipal.IsInRole(_options.User))
                {
                    role = _options.User;
                }
                else if (Thread.CurrentPrincipal.IsInRole(_options.Admin))
                {
                    role = _options.Admin;
                }   
                else
                {
                    // User's role is unknown
                    return Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.unknownRole).ConfigureAwait(false));
                }

                string userHash = (Thread.CurrentPrincipal.Identity as IRoleIdentity).UserHash;

                // Create UserAccount with user's username and role
                IAccount account = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                // Verify if account is enabled and confirmed
                string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(account, cancellationToken).ConfigureAwait(false);

                // Check account verification status, if not return error
                if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess).ConfigureAwait(false)))
                {
                    return Tuple.Create(new List<string>(), resultVerifyAccount);
                }

                // Verify if account is authorized to make changes to Nodes
                string resultVerifyAuthorized = await _accountVerificationService.VerifyAccountAuthorizedNodeChangesAsync(nodeIDs, userHash, cancellationToken).ConfigureAwait(false);

                // Check account verified to make changes status, if not return error
                if (!resultVerifyAuthorized.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess).ConfigureAwait(false)))
                {
                    return Tuple.Create(new List<string>(), resultVerifyAuthorized);
                }

                Tuple<List<string>, string> result = await _tagService.GetNodeTagsAsync(nodeIDs, cancellationToken).ConfigureAwait(false);

                return result;
            }
            catch (OperationCanceledException)
            {
                return Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                return Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message);
            }
        }

        /// <summary>
        ///     CreateTagAsync(tagName)
        ///         Creates tag in tag word bank
        /// </summary>
        /// <param name="tagName">String tag name</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status</returns>
        public async Task<string> CreateTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Check if the user is authenticated
                if (Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated).ConfigureAwait(false);
                } 

                // Check if tag name is null, empty string or all space
                if (tagName == null || tagName.Equals("") || tagName.Trim().Equals(""))
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagNameInvalid).ConfigureAwait(false);
                } 

                // Get user's role
                string role = ""; 
                if (Thread.CurrentPrincipal.IsInRole(_options.User))
                {
                    // User must be an admin to perform changes to tag bank
                    return await _messageBank.GetMessage(IMessageBank.Responses.notAuthorized).ConfigureAwait(false);
                }
                else if (Thread.CurrentPrincipal.IsInRole(_options.Admin))
                {
                    role = _options.Admin;
                }
                else
                {
                    // User has unknown role
                    return await _messageBank.GetMessage(IMessageBank.Responses.unknownRole);
                }
                    

                // UserAccount with user's username and role
                IAccount account = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                // Verify if account is enabled and confirmed
                string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(account, cancellationToken).ConfigureAwait(false);

                // Check if account is enabled and confirmed, if not return error
                if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess)))
                {
                    return resultVerifyAccount;
                }

                string result = await _tagService.CreateTagAsync(tagName, 0, cancellationToken).ConfigureAwait(false);
                
                return result;
            }
            catch (OperationCanceledException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }

        /// <summary>
        ///     RemoveTagAsync(tagName)
        ///         Removes tag from tag bank
        /// </summary>
        /// <param name="tagName">String tag name</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status</returns>
        public async Task<string> RemoveTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                //Check if the user is authenticated
                if (Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated);
                }
                    
                // Check if tag name is null, empty string or all space
                if (tagName == null || tagName.Equals("") || tagName.Trim().Equals(""))
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagNameInvalid).ConfigureAwait(false);
                }

                //Get user's role
                string role = "";
                if (Thread.CurrentPrincipal.IsInRole(_options.User))
                {
                    // User must be admin
                    return await _messageBank.GetMessage(IMessageBank.Responses.notAuthorized);
                }
                else if (Thread.CurrentPrincipal.IsInRole(_options.Admin))
                {
                    role = _options.Admin;
                }
                else
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.unknownRole);
                }

                // Create UserAccount with user's username and role
                IAccount account = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                // Verify if account is enabled and confirmed
                string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(account, cancellationToken);

                // Check if account is enabled and confirme, if not return error
                if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess)))
                {
                    return resultVerifyAccount;
                }

                string result = await _tagService.RemoveTagAsync(tagName, cancellationToken);
                
                return result;
            }
            catch (OperationCanceledException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }

        /// <summary>
        ///     GetTagsAsync()
        ///         Returns a list of tags in tag bank. 
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>List of tags and string status</returns>
        public async Task<Tuple<List<ITag>, string>> GetTagsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Get tags. No need to be authenticated or authorized to retrieve tag bank
                Tuple<List<ITag>, string> result = await _tagService.GetTagsAsync(cancellationToken);

                return result;
            }
            catch (OperationCanceledException)
            {
                return Tuple.Create(new List<ITag>(), await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested));
            }
            catch(Exception ex)
            {
                return Tuple.Create(new List<ITag>(), await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message);
            }
        }
    }
}
