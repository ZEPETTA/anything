using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agora_RTC_Plugin;
using Agora.Rtc;
using Agora.Util;

public class NewBehaviourScript : MonoBehaviour
{
    public string AppID;
    public string ChannelName;

    VideoSurface myView;
    VideoSurface remoteView;
    IRtcEngine mRtcEngine;

    void Awake()
    {
        // SetupUI();
    }

    void Start()
    {
        // SetupAgora();
    }

    void Join()
    { }

    void Leave()
    { }

    void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed)
    { }

    void OnLeaveChannelHandler(RtcStats stats)
    { }

    void OnUserJoined(uint uid, int elapsed)
    { }

    void OnUserOffline(uint uid, USER_OFFLINE_REASON_TYPE reason)
    { }

    void OnApplicationQuit()
    { }
}
