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
         private string txtSendContent = "What on your mind?";
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
                if (!nicknameContent.Equals(value)) { 
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
                        descendingIsChecked=false;
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
        #endregion

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}