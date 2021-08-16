using DataAccessLayer.Common;
using DataAccessLayer.Entities;
using DataAccessLayer.Repository;
using System;
using System.Linq;
using DataAccessLayer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class UserDAL
    {
        private readonly IJwtAuthenticationManager jwtAuthenticationManager;
        private readonly BaatCheetDbContext dbContext;

        public UserDAL(IJwtAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            this.dbContext = new BaatCheetDbContext();
        }
        public UserDAL()
        {
            this.dbContext = new BaatCheetDbContext();
        }

        public GroupChat AddNewGroup(string groupName, int createdBy, ref string message)
        {
            try
            {
                var hash = Commons.Hash();
                Group group = new Group
                {
                    Name = groupName,
                    CreatedBy = createdBy,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = createdBy,
                    ModifiedOn = DateTime.Now,
                    GroupHash = "G-" + hash,
                };
                this.dbContext.Add(group);
                this.dbContext.SaveChanges();

                UserGroup userGroup = new UserGroup
                {
                    UserId = createdBy,
                    GroupId = group.Id,
                    IsAdmin = true,
                };
                this.dbContext.Add(userGroup);
                this.dbContext.SaveChanges();
                message = "Group created successfully";

                return GetGroupDetails(group.Id);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public string ChangePassword(string oldPassword, string newPassword, string token)
        {
            try
            {
                User user = this.dbContext.Users.FirstOrDefault(x => x.Token == Security.GetToken(token));
                if (user == null)
                    return "UnAuthorized user";
                else
                {
                    var oldPasswordHash = Security.HashSHA1WithSalt(oldPassword, user.Email);
                    if (oldPasswordHash == user.Password)
                    {
                        user.Password = Security.HashSHA1WithSalt(newPassword, user.Email);
                        this.dbContext.Update(user);
                        this.dbContext.SaveChanges();
                        return "Password change successfully";
                    }
                }
                return "Old password not recognize";
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        public string GetUserContactEmail(int userContactId, int messageBy)
        {
            var query = this.dbContext.Conversations
                .Join(
                    this.dbContext.UserContacts,
                    conversations => conversations.UserContactId,
                    userContact => userContact.Id,
                    (conversation, userContact) => new
                    {
                        userContact
                    }
                )
                .Join(
                    this.dbContext.Users,
                    userContact => messageBy == userContact.userContact.UserId ? userContact.userContact.ContactId : userContact.userContact.UserId,
                    user => user.Id,
                    (userContact, user) => new {
                        user,
                        userContact
                    }
                )
                .Where(x => x.userContact.userContact.Id == userContactId);

            string email = "";
            foreach (var user in query)
            {
                email = user.user.Email;
            }
            return email;
        }

        public GroupChat AddUserToGroup(string email, int userId, int groupId, ref string message)
        {
            
            var newUser = this.dbContext.Users.FirstOrDefault(x => x.Email == email);
            if (newUser == null)
                message = "No user found";
            else
            {
                var newUserGroup = this.dbContext.UserGroups.FirstOrDefault(x => x.UserId == newUser.Id && x.GroupId == groupId);
                if (newUserGroup != null)
                    message = "User already available in the group";
                else
                {
                    var userGroup = new UserGroup
                    {
                        UserId = newUser.Id,
                        GroupId = groupId,
                    };
                    this.dbContext.Add(userGroup);
                    this.dbContext.SaveChanges();

                    message = "User added in the group successfully";
                }
            }
            return GetGroupDetails(groupId);
        }

        public UserConversation AddNewContact(string email, int userId, ref string message)
        {
            User user = this.dbContext.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                message = "UnAuthorized user";
            else
            {
                var contact = this.dbContext.Users.FirstOrDefault(x => x.Email == email);
                if(contact == null)
                    message = "No user found";
                else
                {
                    var userContact = this.dbContext.UserContacts.FirstOrDefault(x => (x.UserId == userId && x.ContactId == contact.Id) || (x.ContactId == userId && x.UserId == contact.Id));
                    if (userContact != null)
                        message = "User already added";
                    else
                    {
                        var hash = Commons.Hash();
                        var newUserContact = new UserContact
                        {
                            UserId = userId,
                            ContactId = contact.Id,
                            IsBlocked = false,
                            Name = "C-" + hash,
                        };
                        this.dbContext.Add(newUserContact);
                        this.dbContext.SaveChanges();

                        message = "User added successfully";
                    }
                }
            }
            return GetUserDetails(email);
        }

        private object GetUserGroupDetails(int userId)
        {
            List<GroupChat> groupChatList = new List<GroupChat>();

            var userGroups = this.dbContext.UserGroups
                                .Join(
                                    this.dbContext.Groups,
                                    userGroups => userGroups.GroupId,
                                    group => group.Id,
                                    (userGroups, group) => new
                                    {
                                        UserId = userGroups.UserId,
                                        GroupId = group.Id,
                                        GroupName = group.Name,
                                        CreatedBy = new
                                        {
                                            Id = group.CreatedByNavigation.Id,
                                            Name = group.CreatedByNavigation.Name,
                                            Email = group.CreatedByNavigation.Email,
                                            DateOfBirth = group.CreatedByNavigation.DateOfBirth
                                        },
                                        Name = group.GroupHash
                                    }
                                )
                                .Where(x => x.UserId == userId).ToList();

            foreach (var userGroup in userGroups)
            {
                GroupChat groupChat = new GroupChat();
                groupChat.GroupId = userGroup.GroupId;
                groupChat.GroupName = userGroup.GroupName;
                groupChat.CreatedBy.Id = userGroup.CreatedBy.Id;
                groupChat.CreatedBy.Name = userGroup.CreatedBy.Name;
                groupChat.CreatedBy.Email = userGroup.CreatedBy.Email;
                groupChat.CreatedBy.DateOfBirth = userGroup.CreatedBy.DateOfBirth;
                groupChat = GetGroupMembers(groupChat, userGroup.GroupId);
                groupChat = GetGroupConversations(groupChat, userGroup.GroupId);
                
                groupChatList.Add(groupChat);
            }

            return groupChatList;
        }

        private GroupChat GetGroupDetails(int groupId)
        {
            GroupChat groupChat = new GroupChat();

            var userGroups = this.dbContext.UserGroups
                                .Join(
                                    this.dbContext.Groups,
                                    userGroups => userGroups.GroupId,
                                    group => group.Id,
                                    (userGroups, group) => new
                                    {
                                        UserId = userGroups.UserId,
                                        GroupId = group.Id,
                                        GroupName = group.Name,
                                        CreatedBy = new
                                        {
                                            Id = group.CreatedByNavigation.Id,
                                            Name = group.CreatedByNavigation.Name,
                                            Email = group.CreatedByNavigation.Email,
                                            DateOfBirth = group.CreatedByNavigation.DateOfBirth
                                        },
                                    }
                                )
                                .Where(x => x.GroupId == groupId).ToList();

            foreach (var userGroup in userGroups)
            {
                groupChat = new GroupChat();
                groupChat.GroupId = userGroup.GroupId;
                groupChat.GroupName = userGroup.GroupName;
                groupChat.CreatedBy.Id = userGroup.CreatedBy.Id;
                groupChat.CreatedBy.Name = userGroup.CreatedBy.Name;
                groupChat.CreatedBy.Email = userGroup.CreatedBy.Email;
                groupChat.CreatedBy.DateOfBirth = userGroup.CreatedBy.DateOfBirth;
                groupChat = GetGroupMembers(groupChat, userGroup.GroupId);
                groupChat = GetGroupConversations(groupChat, userGroup.GroupId);

            }

            return groupChat;
        }

        private GroupChat GetGroupConversations(GroupChat groupChat, int groupId)
        {
            var groupConversations = this.dbContext.GroupConversations
                                        .Join(
                                            this.dbContext.Users,
                                            groupConversation => groupConversation.MessageBy,
                                            user => user.Id,
                                            (groupConversation, user) => new
                                            {
                                                groupConversation,
                                                user
                                            }
                                        )
                                        .Where(x => x.groupConversation.GroupId == groupId && x.groupConversation.IsDeleted != true)
                                        .Select(
                                            select => new
                                            {
                                                Id = select.groupConversation.Id,
                                                Message = select.groupConversation.Message,
                                                MessageBy = select.user,
                                                MessageOn = select.groupConversation.MessageOn,
                                                IsDeleted = select.groupConversation.IsDeleted,
                                            }
                                        )
                                        .OrderBy(x => x.MessageOn);

            foreach (var groupConversation in groupConversations)
            {
                Chat chat = new Chat();
                chat.Id = groupConversation.Id;
                chat.Message = groupConversation.Message;
                chat.MessageBy = groupConversation.MessageBy.Id;
                chat.MessageOn = groupConversation.MessageOn;
                chat.IsDeleted = groupConversation.IsDeleted;
                groupChat.Chats.Add(chat);
            }

            return groupChat;
        }

        private GroupChat GetGroupMembers(GroupChat groupChat, int groupId)
        {
            var groupMembers = this.dbContext.UserGroups
                                .Join(
                                    this.dbContext.Users,
                                    userGroup => userGroup.UserId,
                                    user => user.Id,
                                    (userGroup, user) => new
                                    {
                                        userGroup,
                                        user,
                                    }
                                )
                                .Where(x => x.userGroup.GroupId == groupId)
                                .Select(
                                    select => new
                                    {
                                        Id = select.user.Id,
                                        Name = select.user.Name,
                                        Email = select.user.Email,
                                        DateOfBirth = select.user.DateOfBirth,
                                        Admin = select.userGroup.IsAdmin,
                                        ImageUrl = select.user.ImageUrl
                                    }
                                );

            foreach (var groupMember in groupMembers)
            {
                Person person = new Person();
                person.Id = groupMember.Id;
                person.Name = groupMember.Name;
                person.DateOfBirth = groupMember.DateOfBirth;
                person.Email = groupMember.Email;
                person.ImageUrl = groupMember.ImageUrl;
                groupChat.GroupMember.Add(person);
                groupChat.IsAdmin = groupMember.Admin;
            }

            return groupChat;
        }

        private object GetUserChatDetails(int userId)
        {

            List<UserConversation> chatConversation = new List<UserConversation>();
            var query = this.dbContext.UserContacts
                            .Join(
                                this.dbContext.Users,
                                userContact => userContact.UserId,
                                user => user.Id,
                                (userContact, user) => new
                                {
                                    UserId = userContact.UserId,
                                    UserContactId = userContact.Id,
                                    ContactId = userContact.ContactId,
                                    ContactName = userContact.Contact.Name,
                                    Email = userContact.Contact.Email,
                                    DateOfBirth = userContact.Contact.DateOfBirth,
                                    Name = userContact.Name,
                                    ImageUrl = userContact.Contact.ImageUrl
                                }
                            )
                            .Where(x => x.UserId == userId)
                            .ToList();
            
            if(query.Count == 0)
                query = this.dbContext.UserContacts
                            .Join(
                                this.dbContext.Users,
                                userContact => userContact.ContactId,
                                user => user.Id,
                                (userContact, user) => new
                                {
                                    UserId = userContact.ContactId,
                                    UserContactId = userContact.Id,
                                    ContactId = userContact.UserId,
                                    ContactName = userContact.User.Name,
                                    Email = userContact.User.Email,
                                    DateOfBirth = userContact.User.DateOfBirth,
                                    Name = userContact.Name,
                                    ImageUrl = userContact.User.ImageUrl
                                }
                            )
                            .Where(x => x.UserId == userId)
                            .ToList();

            foreach (var data in query)
            {
                UserConversation userConversation = new UserConversation();
                userConversation.UserContactId = data.UserContactId;
                userConversation.Person.Id = data.ContactId;
                userConversation.Person.Name = data.ContactName;
                userConversation.Person.Email = data.Email;
                userConversation.Person.DateOfBirth = data.DateOfBirth;
                userConversation.Person.ImageUrl = data.ImageUrl;
                chatConversation.Add(userConversation);
            }

            for (int i = 0; i < chatConversation.Count; ++i)
                chatConversation[i].Messages = this.dbContext.Conversations
                    .Where(x => x.UserContactId == chatConversation[i].UserContactId)
                    .OrderBy(x => x.MessageOn).ToList();
            return chatConversation;
        }

        private UserConversation GetUserDetails(string email)
        {

            UserConversation chatConversation = new UserConversation();
            var query = this.dbContext.UserContacts
                            .Join(
                                this.dbContext.Users,
                                userContact => userContact.UserId,
                                user => user.Id,
                                (userContact, user) => new
                                {
                                    UserId = userContact.UserId,
                                    UserContactId = userContact.Id,
                                    ContactId = userContact.ContactId,
                                    ContactName = userContact.Contact.Name,
                                    Email = userContact.Contact.Email,
                                    DateOfBirth = userContact.Contact.DateOfBirth,
                                    Name = userContact.Name,
                                    ImageUrl = userContact.Contact.ImageUrl
                                }
                            )
                            .Where(x => x.Email == email)
                            .ToList();

            if (query.Count == 0)
                query = this.dbContext.UserContacts
                            .Join(
                                this.dbContext.Users,
                                userContact => userContact.ContactId,
                                user => user.Id,
                                (userContact, user) => new
                                {
                                    UserId = userContact.ContactId,
                                    UserContactId = userContact.Id,
                                    ContactId = userContact.UserId,
                                    ContactName = userContact.User.Name,
                                    Email = userContact.User.Email,
                                    DateOfBirth = userContact.User.DateOfBirth,
                                    Name = userContact.Name,
                                    ImageUrl = userContact.User.ImageUrl
                                }
                            )
                            .Where(x => x.Email == email)
                            .ToList();

            foreach (var data in query)
            {
                chatConversation = new UserConversation();
                chatConversation.UserContactId = data.UserContactId;
                chatConversation.Person.Id = data.ContactId;
                chatConversation.Person.Name = data.ContactName;
                chatConversation.Person.Email = data.Email;
                chatConversation.Person.DateOfBirth = data.DateOfBirth;
                chatConversation.Person.ImageUrl = data.ImageUrl;
            }

            chatConversation.Messages = this.dbContext.Conversations
                .Where(x => x.UserContactId == chatConversation.UserContactId)
                .OrderBy(x => x.MessageOn).ToList();
            return chatConversation;
        }

        public object GetUserDetails(string authorization, ref string message)
        {
            var user = this.dbContext.Users.FirstOrDefault(x => x.Token == Security.GetToken(authorization));
            if (user == null)
                message = "UnAuthorized user";
            else
                return GetDetails(user);
            return null;
        }

        private object GetDetails(User user)
        {
            UserInfo userInfo = new UserInfo();
            userInfo.Person.Id = user.Id;
            userInfo.Person.Name = user.Name;
            userInfo.Person.FirstName = user.FirstName;
            userInfo.Person.LastName = user.LastName;
            userInfo.Person.MiddleName = user.MiddleName;
            userInfo.Person.Email = user.Email;
            userInfo.Person.DateOfBirth = user.DateOfBirth;
            userInfo.Person.ImageUrl = user.ImageUrl;

            userInfo.ChatDetails = GetUserChatDetails(user.Id);
            userInfo.GroupDetails = GetUserGroupDetails(user.Id);
            return userInfo;
        }

        public string RegisterUser(User user)
        {
            var newUser = this.dbContext.Users.FirstOrDefault(x => x.Email == user.Email);
            if(newUser == null)
            {
                user.Password = Security.HashSHA1WithSalt(user.Password, user.Email);
                this.dbContext.Add(user);
                this.dbContext.SaveChanges();
                return "User created";
            }
            else
                return "Email is already registered";
        }

        public string AuthenticateUser(string email, string password)
        {
            try
            {
                password = Security.HashSHA1WithSalt(password, email);
                var user = this.dbContext.Users.FirstOrDefault(x => x.Email == email && x.Password == password);
                if (user == null)
                    return null;

                var token = jwtAuthenticationManager.Authenticate(email, Security.HashSHA1WithSalt(password, email));
                if (token == null)
                    return null;

                var entity = this.dbContext.Users.FirstOrDefault(x => x.Id == user.Id);
                if (entity != null)
                {
                    entity.Token = token;
                    entity.ModifiedOn = DateTime.Now;
                    this.dbContext.SaveChanges();
                }

                return token;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            
        }
    }
}
