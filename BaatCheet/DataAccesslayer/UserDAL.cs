using DataAccessLayer.Common;
using DataAccessLayer.Entities;
using DataAccessLayer.Repository;
using System;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Model;
using System.Collections.Generic;

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

        private static ulong Hash()
        {
            ulong kind = (ulong)(int)DateTime.Now.Kind;
            return (kind << 62) | (ulong)DateTime.Now.Ticks;
        }

        public object AddNewGroup(string groupName, string userId, string token, ref string message)
        {
            User user = Security.AuthenticateUser(this.dbContext, int.Parse(userId), token);
            if (user == null)
                message = "UnAuthorized user";
            else
            {
                try
                {
                    var hash = Hash();
                    Group group = new Group
                    {
                        Name = groupName,
                        CreatedBy = int.Parse(userId),
                        CreatedOn = DateTime.Now,
                        ModifiedBy = int.Parse(userId),
                        ModifiedOn = DateTime.Now,
                        GroupHash = "G-" + Hash(),
                    };
                    this.dbContext.Add(group);
                    this.dbContext.SaveChanges();

                    UserGroup userGroup = new UserGroup
                    {
                        UserId = int.Parse(userId),
                        GroupId = group.Id,
                        IsAdmin = true,
                    };
                    this.dbContext.Add(userGroup);
                    this.dbContext.SaveChanges();
                    message = "Group created successfully";
                    return getDetails(user);
                }
                catch(Exception ex)
                {
                    return ex.Message;
                }
            }
            return null;
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

        public void AddUserToGroup(string email, string userId, string groupId, string token, ref string message)
        {
            var user = Security.AuthenticateUser(this.dbContext, int.Parse(userId), token);
            if (user == null)
                message = "UnAuthorized user";
            else
            {
                var newUser = this.dbContext.Users.FirstOrDefault(x => x.Email == email);
                if (newUser == null)
                    message = "No user found";
                else
                {
                    var newUserGroup = this.dbContext.UserGroups.FirstOrDefault(x => x.UserId == newUser.Id && x.GroupId == int.Parse(groupId));
                    if (newUserGroup != null)
                        message = "User already available in the group";
                    else
                    {
                        var userGroup = new UserGroup
                        {
                            UserId = newUser.Id,
                            GroupId = int.Parse(groupId),
                        };
                        this.dbContext.Add(userGroup);
                        this.dbContext.SaveChanges();

                        message = "User added in the group successfully";
                    }
                }
            }
        }

        public object AddNewContact(string email, string userId, string token, ref string message)
        {
            User user = Security.AuthenticateUser(this.dbContext, int.Parse(userId), token);
            if (user == null)
                message = "UnAuthorized user";
            else
            {
                var contact = this.dbContext.Users.FirstOrDefault(x => x.Email == email);
                if(contact == null)
                    message = "No user found";
                else
                {
                    var userContact = this.dbContext.UserContacts.FirstOrDefault(x => (x.UserId == int.Parse(userId) && x.ContactId == contact.Id) || (x.ContactId == int.Parse(userId) && x.UserId == contact.Id));
                    if (userContact != null)
                        message = "User already added";
                    else
                    {
                        var hash = Hash();
                        var newUserContact = new UserContact
                        {
                            UserId = int.Parse(userId),
                            ContactId = contact.Id,
                            IsBlocked = false,
                            Name = "C-" + hash,
                        };
                        this.dbContext.Add(newUserContact);
                        this.dbContext.SaveChanges();

                        message = "User added successfully";
                        return getDetails(user);
                    }
                }
            }
            return null;
        }

        private object getUserGroupDetails(int userId)
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
                groupChat.Name = userGroup.Name;
                groupChat.CreatedBy.Id = userGroup.CreatedBy.Id;
                groupChat.CreatedBy.Name = userGroup.CreatedBy.Name;
                groupChat.CreatedBy.Email = userGroup.CreatedBy.Email;
                groupChat.CreatedBy.DateOfBirth = userGroup.CreatedBy.DateOfBirth;
                groupChat = getGroupMembers(groupChat, userGroup.GroupId);
                groupChat = getGroupConversations(groupChat, userGroup.GroupId);
                
                groupChatList.Add(groupChat);
            }

            return groupChatList;
        }

        private GroupChat getGroupConversations(GroupChat groupChat, int groupId)
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

        private GroupChat getGroupMembers(GroupChat groupChat, int groupId)
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
                                        Admin = select.userGroup.IsAdmin
                                    }
                                );

            foreach (var groupMember in groupMembers)
            {
                Person person = new Person();
                person.Id = groupMember.Id;
                person.Name = groupMember.Name;
                person.DateOfBirth = groupMember.DateOfBirth;
                person.Email = groupMember.Email;
                groupChat.GroupMember.Add(person);
                groupChat.IsAdmin = groupMember.Admin;
            }

            return groupChat;
        }

        private object getUserChatDetails(int userId)
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
                                    Name = userContact.Name
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
                                    Name = userContact.Name
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
                chatConversation.Add(userConversation);
            }

            for (int i = 0; i < chatConversation.Count; ++i)
                chatConversation[i].Messages = this.dbContext.Conversations
                    .Where(x => x.UserContactId == chatConversation[i].UserContactId)
                    .OrderBy(x => x.MessageOn).ToList();
            return chatConversation;
        }

        public object GetUserDetails(string authorization, ref string message)
        {
            var user = this.dbContext.Users.FirstOrDefault(x => x.Token == Security.getToken(authorization));
            if (user == null)
                message = "UnAuthorized user";
            else
                return getDetails(user);
            return null;
        }

        private object getDetails(User user)
        {
            UserInfo userInfo = new UserInfo();
            userInfo.Person.Id = user.Id;
            userInfo.Person.Name = user.Name;
            userInfo.Person.FirstName = user.FirstName;
            userInfo.Person.LastName = user.LastName;
            userInfo.Person.MiddleName = user.MiddleName;
            userInfo.Person.Email = user.Email;
            userInfo.Person.DateOfBirth = user.DateOfBirth;

            userInfo.ChatDetails = getUserChatDetails(user.Id);
            userInfo.GroupDetails = getUserGroupDetails(user.Id);
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
            var user = this.dbContext.Users.FirstOrDefault(x => x.Email == email && x.Password == Security.HashSHA1WithSalt(password, email));
            if (user == null)
                return null;

            var token = jwtAuthenticationManager.Authenticate(email, Security.HashSHA1WithSalt(password, email));
            if (token == null)
                return null;

            var entity = this.dbContext.Users.FirstOrDefault(x => x.Id == user.Id);
            if(entity != null)
            {
                entity.Token = token;
                entity.ModifiedOn = DateTime.Now;
                this.dbContext.SaveChanges();
            }

            return token;
        }
    }
}
