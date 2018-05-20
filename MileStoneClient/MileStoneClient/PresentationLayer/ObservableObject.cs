using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MileStoneClient.PresentationLayer
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableObject()
        {
            Messages.CollectionChanged += Messages_CollectionChanged;
        }

        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();
        private void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Messages");
        }


        #region ChatRoomWindowBinding

        /// <summary>
        /// A binding function that connects between the "Send" textbox to source
        /// </summary
        private string txtSendContent = "";
        public string TxtSendContent
        {
            get
            {
                return txtSendContent;
            }
            set
            {
                if (!txtSendContent.Equals(value))
                {
                    txtSendContent = value;
                    OnPropertyChanged("txtSendContent");
                }
            }
        }


        /// <summary>
        /// A binding function that connects between the "options" button to source
        /// </summary
        private object isOptionVisible = null;
        public object IsOptionVisible
        {
            get
            {
                return isOptionVisible;
            }
            set
            {
                if (isOptionVisible != value)
                {
                    isOptionVisible = value;
                    OnPropertyChanged("IsOptionVisible");
                }
            }
        }
        #endregion

        #region LoginWindowBinding

        /// <summary>
        /// A binding function that connects between the "Group Id" textbox to source
        /// </summary>
        private string groupIdContent = "";
        public string GroupIdContent
        {
            get
            {
                return groupIdContent;
            }
            set
            {
                groupIdContent = value;
                OnPropertyChanged("groupIdContent");
            }
        }

        /// <summary>
        /// A binding function that connects between the "NickName" textbox to source
        /// </summary>
        private string nicknameContent = "";
        public string NicknameContent
        {
            get
            {
                return nicknameContent;
            }
            set
            {
                if (!nicknameContent.Equals(value))
                {
                    nicknameContent = value;
                    OnPropertyChanged("nicknameContent");
                }
            }
        }

        /// <summary>
        /// A binding function that connects between the source to "LblAddReg" label 
        /// </summary>
        private string lblAddRegContent = "";
        public string LblAddRegContent
        {
            get
            {
                return lblAddRegContent;
            }
            set
            {
                lblAddRegContent = value;
                OnPropertyChanged("LblAddRegContent");
            }
        }

        /// <summary>
        /// A binding function that connects between the source to "BtnRegister" visibility button 
        /// </summary>
        private string btnRegisterVisibility = "Hidden";
        public string BtnRegisterVisibility
        {
            get
            {
                return btnRegisterVisibility;
            }
            set
            {
                btnRegisterVisibility = value;
                OnPropertyChanged("BtnRegisterVisibility");
            }
        }

        /// <summary>
        /// A binding function that connects between the source to "LblAddReg" visibility label 
        /// </summary>
        private string lblAddRegVisibility = "Hidden";
        public string LblAddRegVisibility
        {
            get
            {
                return lblAddRegVisibility;
            }
            set
            {
                lblAddRegVisibility = value;
                OnPropertyChanged("LblAddRegVisibility");
            }
        }

        /// <summary>
        /// A binding function that connects between the source to "BtnLogin" enabled button 
        /// </summary>
        private bool btnLoginIsEnabled = false;
        public bool BtnLoginIsEnabled
        {
            get
            {
                return btnLoginIsEnabled;
            }
            set
            {
                btnLoginIsEnabled = value;
                OnPropertyChanged("BtnLoginIsEnabled");
            }
        }

    #endregion

        #region RegisterWindowBinding

    /// <summary>
    /// A binding function that connects between the "btnReg" button to source
    /// </summary>
    private bool btnRegIsEnabled = false;
        public bool BtnRegIsEnabled
        {
            get
            {
                return btnRegIsEnabled;
            }
            set
            {
                btnRegIsEnabled = value;
                OnPropertyChanged("BtnRegIsEnabled");
            }
        }

        /// <summary>
        /// A binding function that connects between the "Group Id" textbox to source
        /// </summary>
        private string groupIdText = "";
        public string GroupIdText
        {
            get
            {
                return groupIdText;
            }
            set
            {
                groupIdText = value;
                OnPropertyChanged("GroupIdText");
            }
        }

        /// <summary>
        /// A binding function that connects between the "Nickname" textbox to source
        /// </summary>
        private string nicknameText = "";
        public string NicknameText
        {
            get
            {
                return nicknameText;
            }
            set
            {
                nicknameText = value;
                OnPropertyChanged("NicknameText");
            }
        }

        /// <summary>
        /// A binding function that connects between the "LblRegError" label to source
        /// </summary>
        private string lblRegErrorVisibility = "Hidden";
        public string LblRegErrorVisibility
        {
            get
            {
                return lblRegErrorVisibility;
            }
            set
            {
                lblRegErrorVisibility = value;
                OnPropertyChanged("LblRegErrorVisibility");
            }
        }

        /// <summary>
        /// A binding function that connects between the "LblRegErrorContent" label to source
        /// </summary>
        private string lblRegErrorContent = "";
        public string LblRegErrorContent
        {
            get
            {
                return lblRegErrorContent;
            }
            set
            {
                lblRegErrorContent = value;
                OnPropertyChanged("LblRegErrorContent");
            }
        }

        /// <summary>
        /// A binding function that connects between the "btnLogin" button to source
        /// </summary>
        private string btnLoginVisibility = "Hidden";
        public string BtnLoginVisibility
        {
            get
            {
                return btnLoginVisibility;
            }
            set
            {
                btnLoginVisibility = value;
                OnPropertyChanged("BtnLoginVisibility");
            }
        }

        /// <summary>
        /// A binding function that connects between the "lblAddLoginContent" label to source
        /// </summary>
        private string lblAddLoginContent = "";
        public string LblAddLoginContent
        {
            get
            {
                return lblAddLoginContent;
            }
            set
            {
                lblAddLoginContent = value;
                OnPropertyChanged("LblAddLoginContent");
            }
        }

        /// <summary>
        /// A binding function that connects between the "LblAddLoginVisibility" label to source
        /// </summary>
        private string lblAddLoginVisibility = "Hidden";
        public string LblAddLoginVisibility
        {
            get
            {
                return lblAddLoginVisibility;
            }
            set
            {
                lblAddLoginVisibility = value;
                OnPropertyChanged("LblAddLoginVisibility");
            }
        }

        #endregion

        #region OptionsWindowBinding

        /// <summary>
        /// A binding function that connects between the source to "Ascending" button 
        /// </summary>
        private bool ascendingIsChecked = false;
        public bool AscendingIsChecked
        {
            get
            {
                return ascendingIsChecked;
            }
            set
            {
                if (ascendingIsChecked != value)
                {
                    //force that only one button will be pressed
                    if (descendingIsChecked & value)
                        descendingIsChecked = false;
                    ascendingIsChecked = value;
                    OnPropertyChanged("AscendingIsChecked");
                }
            }
        }
        /// <summary>
        /// A binding function that connects between the source to "Descending" button 
        /// </summary>
        private bool descendingIsChecked = false;
        public bool DescendingIsChecked
        {
            get
            {
                return descendingIsChecked;
            }
            set
            {
                if (descendingIsChecked != value)
                {
                    //force that only one button will be pressed
                    if (ascendingIsChecked & value)
                        ascendingIsChecked = false;
                    descendingIsChecked = value;
                    OnPropertyChanged("DescendingIsChecked");
                }
            }
        }

        #region filters
        /// <summary>
        /// A binding function that connects between the source to "None" filter button 
        /// </summary>
        private bool f_NoneIsChecked = false;
        public bool F_NoneIsChecked
        {
            get
            {
                return f_NoneIsChecked;
            }
            set
            {
                if (f_NoneIsChecked != value)
                {
                    //force that only one button is pressed for filters
                    if (value & (f_UserIsChecked | f_GroupIsChecked))
                    {
                        if (f_UserIsChecked)
                            f_UserIsChecked = false;
                        else f_GroupIsChecked = false;
                    }
                    f_NoneIsChecked = value;
                    OnPropertyChanged("F_NoneIsChecked");
                }
            }
        }

        /// <summary>
        /// A binding function that connects between the source to "User" filter button 
        /// </summary>
        private bool f_UserIsChecked = false;
        public bool F_UserIsChecked
        {
            get
            {
                return f_UserIsChecked;
            }
            set
            {
                if (f_UserIsChecked != value)
                {
                    //force that only one button is pressed for filters
                    if (value & (f_NoneIsChecked | f_GroupIsChecked))
                    {
                        if (f_NoneIsChecked)
                            f_NoneIsChecked = false;
                        else f_GroupIsChecked = false;
                    }
                    f_UserIsChecked = value;
                    OnPropertyChanged("F_UserIsChecked");
                }
            }
        }

        /// <summary>
        /// A binding function that connects between the source to "Group Id" filter button 
        /// </summary>
        private bool f_GroupIsChecked = false;
        public bool F_GroupIsChecked
        {
            get
            {
                return f_GroupIsChecked;
            }
            set
            {
                if (f_GroupIsChecked != value)
                {
                    //force that only one button is pressed for filters
                    if (value & (f_NoneIsChecked | f_UserIsChecked))
                    {
                        if (f_NoneIsChecked)
                            f_NoneIsChecked = false;
                        else f_UserIsChecked = false;
                    }
                    f_GroupIsChecked = value;
                    OnPropertyChanged("F_GroupIsChecked");
                }
            }
        }
        #endregion

        #region sorts
        /// <summary>
        /// A binding function that connects between the source to "Group Id, NickName, Timestemp" button 
        /// </summary>
        private bool s_AllIsChecked = false;
        public bool S_AllIsChecked
        {
            get
            {
                return s_AllIsChecked;
            }
            set
            {
                if (s_AllIsChecked != value)
                {
                    //force that only one button is pressed for sort
                    if (value & (s_TimeIsChecked | s_NicknameIsChecked))
                    {
                        if (s_TimeIsChecked)
                            s_TimeIsChecked = false;
                        else s_NicknameIsChecked = false;
                    }
                    s_AllIsChecked = value;
                    OnPropertyChanged("S_AllIsChecked");
                }
            }
        }

        /// <summary>
        /// A binding function that connects between the source to "Timestemp" button 
        /// </summary>
        private bool s_TimeIsChecked = false;
        public bool S_TimeIsChecked
        {
            get
            {
                return s_TimeIsChecked;
            }
            set
            {
                if (s_TimeIsChecked != value)
                {
                    //force that only one button is pressed for sort
                    if (value & (s_AllIsChecked | s_NicknameIsChecked))
                    {
                        if (s_AllIsChecked)
                            s_AllIsChecked = false;
                        else s_NicknameIsChecked = false;
                    }
                    s_TimeIsChecked = value;
                    OnPropertyChanged("S_TimeIsChecked");
                }
            }
        }

        /// <summary>
        /// A binding function that connects between the source to "Timestemp" button 
        /// </summary>
        private bool s_NicknameIsChecked = false;
        public bool S_NicknameIsChecked
        {
            get
            {
                return s_NicknameIsChecked;
            }
            set
            {
                if (s_NicknameIsChecked != value)
                {
                    //force that only one button is pressed for sort
                    if (value & (s_TimeIsChecked | s_AllIsChecked))
                    {
                        if (s_TimeIsChecked)
                            s_TimeIsChecked = false;
                        else s_AllIsChecked = false;
                    }
                    s_NicknameIsChecked = value;
                    OnPropertyChanged("S_NicknameIsChecked");
                }
            }
        }
        #endregion

        /// <summary>
        /// A binding function that connects between the source to the visibility of groups filter
        /// </summary>
        private string isGroupFiltered = "Hidden";
        public string IsGroupFiltered
        {
            get
            {
                return isGroupFiltered;
            }
            set
            {
                if (isGroupFiltered != value)
                {
                    isGroupFiltered = value;
                    OnPropertyChanged("IsGroupFiltered");
                }
            }
        }

        /// <summary>
        /// A binding function that connects between the source to the visibility of users filter
        /// </summary>
        private string isUserFiltered = "Hidden";
        public string IsUserFiltered
        {
            get
            {
                return isUserFiltered;
            }
            set
            {
                if (isUserFiltered != value)
                {
                    isUserFiltered = value;
                    OnPropertyChanged("IsUserFiltered");
                }
            }
        }
        #endregion

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}