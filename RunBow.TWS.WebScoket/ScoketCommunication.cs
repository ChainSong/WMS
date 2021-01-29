using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBow.TWS.WebScoket
{
    public class ScoketCommunication
    {

        public List<IWebSocketConnection> allSockets = new List<IWebSocketConnection>();
        public WebSocketServer server = new WebSocketServer("ws://127.0.0.1:8089/");

        private event EventHandler _onConnect;//连接事件
        public event EventHandler OnConnect
        {
            add
            {
                _onConnect += value;
            }
            remove
            {
                _onConnect -= value;
            }
        }

        private event EventHandler<string> _onReceive;//接收前台事件
        public event EventHandler<string> OnReceive
        {
            add
            {
                _onReceive += value;
            }
            remove
            {
                _onReceive -= value;
            }
        }

        private event EventHandler _onClose;
        public event EventHandler OnClose
        {
            add
            {
                _onClose += value;
            }
            remove
            {
                _onClose -= value;
            }
        }

        /// <summary>
        /// 初始化scoketserver
        /// </summary>
        public void InitializationWebScoket()
        {
            
            server.Start(scoket =>
            {
                //监听前台的连接
                scoket.OnOpen = () =>
                {
                    if (_onConnect != null)
                    {
                        _onConnect.Invoke(this, new EventArgs());
                    }

                    OpenWebScoket(scoket);
                };
                //监听前台的关闭
                scoket.OnClose = () =>
                {
                    CloseWebScoket(scoket);
                };
                //监听前台发来的消息
                scoket.OnMessage = message =>
                {
                    if (_onReceive != null)
                    {
                        _onReceive.Invoke(this, message);
                    }
                };
            });

        }


        /// <summary>
        /// 前端发出连接请求
        /// </summary>
        /// <param name="scoket"></param>
        public void OpenWebScoket(IWebSocketConnection scoket)
        {

            try
            {
                allSockets.Add(scoket);
                scoket.Send("0.00");
                //if (_onConnect != null)
                //{
                //    _onConnect.Invoke(this, new EventArgs());
                //}
            }
            catch (Exception)
            {

            }

        }

        /// <summary>
        /// 前端断开连接
        /// </summary>
        /// <param name="scoket"></param>
        public void CloseWebScoket(IWebSocketConnection scoket)
        {
            try
            {
                if (allSockets.Count() > 0)
                {
                    allSockets.Remove(scoket);
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 发送数据到前端网页
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        public async void SendMessageAsync(string weight)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (allSockets.Count() > 0)
                    {
                        foreach (var item in allSockets)
                        {
                            item.Send(weight);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            });

        }

        /// <summary>
        /// 发送数据到前端网页
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        public void SendMessage(string weight)
        {
            try
            {
                if (allSockets.Count() > 0)
                {
                    foreach (var item in allSockets)
                    {
                        item.Send(weight);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


    }
}
