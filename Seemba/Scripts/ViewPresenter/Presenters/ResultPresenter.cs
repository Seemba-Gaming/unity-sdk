﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPresenter : MonoBehaviour
{
    #region static
    #endregion

    #region Script Parameters   
    public Text                     Title;
    public Text                     Subtitle;
    public Text                     DescriptionTitle;
    public Text                     DescriptionSubtite;
    public Text                     Date;
    public Text                     Id;
    public Text                     CurrentUserScore;
    public Text                     OpponentScore;
    public Text                     EntryFee;
    public Text                     CurrentUserName;
    public Text                     OpponentUserName;
    public Text                     CurrentUserScoreText;
    public Text                     OpponentScoreText;
    public Image                    CurrentUserAvatar;
    public Image                    OpponentAvatar;
    public Image                    ResultImage;
    public Button                   PlayAgainButton;
    public Button                   ContinueButton;
    public Sprite[]                 ResultImages;
    #endregion

    #region Fields
    private Challenge               mCurrentChallenge;
    #endregion

    #region Unity Methods
    private void OnEnable()
    {
        var mCurrentChallenge = ChallengeManager.CurrentChallenge;
        if (mCurrentChallenge.user_2_score == null || mCurrentChallenge.user_1_score == null)
        {
            InitResultWaiting(mCurrentChallenge);
        }
        else
        {
            if (mCurrentChallenge.user_1_score == mCurrentChallenge.user_2_score)
            {
                InitResultDraw(mCurrentChallenge);
            }
            else if (mCurrentChallenge.winner_user == UserManager.Get.getCurrentUserId())
            {
                InitResultWin(mCurrentChallenge);
            }
            else
            {
                InitResultLose(mCurrentChallenge);
            }
        }
    }
    #endregion
    #region Methods
    public async void InitResultLose(Challenge challenge)
    {
        Init(challenge);
        Title.text = ResultController.YOU_LOST_TEXT;
        Subtitle.text = ResultController.DEFEAT_TEXT;
        DescriptionTitle.text = ResultController.YOUR_OPPONENT_WON_TEXT;
        DescriptionSubtite.text = ResultController.DO_NOT_GIVE_UP_TEXT;
        ResultImage.sprite = ResultImages[0];

        if (challenge.matched_user_1._id == UserManager.Get.getCurrentUserId())
        {
            OpponentUserName.text = challenge.matched_user_2.username;
            OpponentAvatar.sprite = await UserManager.Get.getAvatar(challenge.matched_user_2.avatar);
            OpponentScore.text = challenge.user_2_score.ToString();

        }
        else
        {
            OpponentUserName.text = challenge.matched_user_1.username;
            OpponentAvatar.sprite = await UserManager.Get.getAvatar(challenge.matched_user_1.avatar);
            OpponentScore.text = challenge.user_1_score.ToString();
        }
    }
    public async void InitResultWin(Challenge challenge)
    {
        Init(challenge);
        Title.text = ResultController.YOU_WIN_TEXT;
        Subtitle.text = ResultController.VICTORY_TEXT;
        DescriptionTitle.text = ResultController.VICTORY_TEXT;
        DescriptionSubtite.text = ResultController.CONGRATS_TEXT;
        ResultImage.sprite = ResultImages[1];
        if (challenge.matched_user_1._id == UserManager.Get.getCurrentUserId())
        {
            OpponentUserName.text = challenge.matched_user_2.username;
            OpponentAvatar.sprite = await UserManager.Get.getAvatar(challenge.matched_user_2.avatar);
            OpponentScore.text = challenge.user_2_score.ToString();

        }
        else
        {
            OpponentUserName.text = challenge.matched_user_1.username;
            OpponentAvatar.sprite = await UserManager.Get.getAvatar(challenge.matched_user_1.avatar);
            OpponentScore.text = challenge.user_1_score.ToString();
        }

    }
    public async void InitResultDraw(Challenge challenge)
    {
        Init(challenge);
        Title.text = ResultController.DRAW_TEXT;
        Subtitle.text = ResultController.TRY_AGAIN;
        DescriptionTitle.text = ResultController.YOU_WILL_WIN;
        DescriptionSubtite.text = ResultController.NEVER_GIVE_UP_TEXT;
        ResultImage.sprite = ResultImages[0];

        if (challenge.matched_user_1._id == UserManager.Get.getCurrentUserId())
        {
            OpponentUserName.text = challenge.matched_user_2.username;
            OpponentAvatar.sprite = await UserManager.Get.getAvatar(challenge.matched_user_2.avatar);
            OpponentScore.text = challenge.user_2_score.ToString();

        }
        else
        {
            OpponentUserName.text = challenge.matched_user_1.username;
            OpponentAvatar.sprite = await UserManager.Get.getAvatar(challenge.matched_user_1.avatar);
            OpponentScore.text = challenge.user_1_score.ToString();
        }
    }
    public void InitResultWaiting(Challenge challenge)
    {
        Init(challenge);
        Title.text = ResultController.WAITING_FOR;
        Subtitle.text = ResultController.PLAYER_2;
        DescriptionTitle.text = ResultController.CONTINUE_NOW;
        DescriptionSubtite.text = ResultController.AND_GET_RESULT_LATER;
        ResultImage.sprite = ResultImages[1];

    }
    public async void Init(Challenge challenge)
    {
        mCurrentChallenge = challenge;
        Date.text = challenge.CreatedAt.Substring(0, challenge.CreatedAt.IndexOf("T"));
        Id.text = challenge._id;
        CurrentUserScore.text = (challenge.matched_user_1._id == UserManager.Get.getCurrentUserId()) ? challenge.user_1_score.ToString():challenge.user_2_score.ToString();
        EntryFee.text = ChallengeManager.Get.GetChallengeFee(float.Parse(challenge.gain), challenge.gain_type).ToString();
        PlayAgainButton.GetComponentInChildren<Text>().text = ResultController.PLAY_AGAIN_TEXT;
        ContinueButton.GetComponentInChildren<Text>().text = ResultController.CONTINUE_TEXT;
        CurrentUserName.text = UserManager.Get.CurrentUser.username;
        CurrentUserAvatar.sprite = await UserManager.Get.getAvatar(UserManager.Get.CurrentUser.avatar);
        if (challenge.gain_type.Equals(ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES))
        {
            EntryFee.text += ResultController.BUBBLES;
        }
        else
        {
            EntryFee.text += CurrencyManager.CURRENT_CURRENCY;
        }
    }

    public void OnClickContinue()
    {
        ViewsEvents.Get.SelectHomeCoroutineLauncher();
    }

    public void OnClickPlayAgain()
    {
        object[] _params = { ChallengeManager.Get.GetChallengeFee(float.Parse(mCurrentChallenge.gain), mCurrentChallenge.gain_type), mCurrentChallenge.gain, mCurrentChallenge.gain_type, ChallengeManager.CHALLENGE_TYPE_1V1 };
        PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
    }

    #endregion
}
