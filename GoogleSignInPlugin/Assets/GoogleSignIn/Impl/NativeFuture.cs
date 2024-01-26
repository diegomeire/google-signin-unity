// <copyright file="NativeFuture.cs" company="Google Inc.">
// Copyright (C) 2017 Google Inc. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>

namespace Google.Impl {
  using System;
  using System.Runtime.InteropServices;
    using UnityEngine;

  /// <summary>
  /// Native future is an interal class that implements the FutureAPIImpl
  /// by calling native methods which are implemented in the native code.
  /// </summary>
  internal class NativeFuture : BaseObject, FutureAPIImpl<GoogleSignInUser> {

    internal NativeFuture(IntPtr ptr) : base(ptr) {
    }

    public override void Dispose() {
      GoogleSignInImpl.GoogleSignIn_DisposeFuture(SelfPtr());
      base.Dispose();
    }

    public bool Pending {
      get {
        return GoogleSignInImpl.GoogleSignIn_Pending(SelfPtr());
      }
    }

    public GoogleSignInUser Result {
      get {

#if UNITY_IOS

IntPtr resultPtr = GoogleSignInImpl.GoogleSignIn_Result(SelfPtr());
        IntPtr userPtr = GoogleSignInImpl.GoogleSignIn_ResultUser(SelfPtr());
        if (resultPtr != IntPtr.Zero) {
           GoogleSignInUser user = new GoogleSignInUser();
           
           GoogleSignInResult result = new GoogleSignInResult();
           
           HandleRef resultRef = new HandleRef(result, resultPtr);
           
           HandleRef userRef = new HandleRef(user, userPtr);
           
           user.AuthCode = OutParamsToString((out_string, out_size) =>
           GoogleSignInImpl.GoogleSignIn_GetServerAuthCode(resultRef, out_string,
                                                     out_size));
           
           user.DisplayName = OutParamsToString((out_string, out_size) =>
           GoogleSignInImpl.GoogleSignIn_GetDisplayName(userRef,
                                                     out_string,
                                                     out_size));
           
           user.Email = OutParamsToString((out_string, out_size) =>
           GoogleSignInImpl.GoogleSignIn_GetEmail(userRef,
                                                 out_string,
                                                 out_size));
           
           user.FamilyName = OutParamsToString((out_string, out_size) =>
           GoogleSignInImpl.GoogleSignIn_GetFamilyName(userRef, out_string,
                                                           out_size));
           
           user.GivenName = OutParamsToString((out_string, out_size) =>
           GoogleSignInImpl.GoogleSignIn_GetGivenName(userRef, out_string,
                                                          out_size));
           
           user.IdToken = OutParamsToString((out_string, out_size) =>
               GoogleSignInImpl.GoogleSignIn_GetIdToken(userRef, out_string,
                                                        out_size));
           
           string url = OutParamsToString((out_string, out_size) =>
           GoogleSignInImpl.GoogleSignIn_GetImageUrl(userRef, out_string,
                                                     out_size));
           
           if ((url != null) && (url.Length > 0)) {
             user.ImageUrl = new System.Uri(url);
           }
           
           user.UserId = OutParamsToString((out_string, out_size) =>
               GoogleSignInImpl.GoogleSignIn_GetUserId(userRef, out_string,
                                                       out_size));
#elif UNITY_ANDROID

        IntPtr userPtr = GoogleSignInImpl.GoogleSignIn_Result(SelfPtr());
        if (userPtr != IntPtr.Zero)
        {
            GoogleSignInUser user = new GoogleSignInUser();

            HandleRef userRef = new HandleRef(user, userPtr);

            user.AuthCode = OutParamsToString((out_string, out_size) =>
            GoogleSignInImpl.GoogleSignIn_GetServerAuthCode(userRef, out_string,
                                                        out_size));

            user.DisplayName = OutParamsToString((out_string, out_size) =>
            GoogleSignInImpl.GoogleSignIn_GetDisplayName(userRef,
                                                        out_string,
                                                        out_size));

            user.Email = OutParamsToString((out_string, out_size) =>
            GoogleSignInImpl.GoogleSignIn_GetEmail(userRef,
                                                    out_string,
                                                    out_size));

            user.FamilyName = OutParamsToString((out_string, out_size) =>
            GoogleSignInImpl.GoogleSignIn_GetFamilyName(userRef, out_string,
                                                            out_size));

            user.GivenName = OutParamsToString((out_string, out_size) =>
            GoogleSignInImpl.GoogleSignIn_GetGivenName(userRef, out_string,
                                                            out_size));

            user.IdToken = OutParamsToString((out_string, out_size) =>
            GoogleSignInImpl.GoogleSignIn_GetIdToken(userRef, out_string,
                                                            out_size));

            string url = OutParamsToString((out_string, out_size) =>
            GoogleSignInImpl.GoogleSignIn_GetImageUrl(userRef, out_string,
                                                        out_size));

            if ((url != null) && (url.Length > 0))
            {
                user.ImageUrl = new System.Uri(url);
            }

            user.UserId = OutParamsToString((out_string, out_size) =>
            GoogleSignInImpl.GoogleSignIn_GetUserId(userRef, out_string,
                                                        out_size));

#endif
          return user;
        } else {
          return null;
        }
      }
    }

    /// <summary>
    /// Gets the status.
    /// </summary>
    /// <remarks>The platform specific implementation maps the platform specific
    /// code to one defined in GoogleSignStatusCode.</remarks>
    /// <value>The status.</value>
    public GoogleSignInStatusCode Status {
      get {
        return (GoogleSignInStatusCode)GoogleSignInImpl.GoogleSignIn_Status(
          SelfPtr());
      }
    }
  }
}
