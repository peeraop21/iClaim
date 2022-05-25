<template>
    <div>
        <b-navbar class="navbar-digital-cliam justify-content-between mb-4" style="">
            <div align="left" style="width: 100%;">
                <ion-icon name="chevron-back-outline" align="left" style=" margin-bottom: -4px; padding-right: 20px; font-size: 23px; color: white;" @click="onClickBackBtn"></ion-icon>
                <label align="left" class="b-navbar-brand mt-1">iClaim</label>
            </div>
            <!--<router-link to="/"><ion-icon name="chevron-back-outline" style="font-size: 23px; color: white; margin-top: 10px;"></ion-icon></router-link>
            <p class="b-navbar-brand mt-3">Digital Claim</p>-->
            <router-link to="/"><ion-icon name="home" style="font-size: 23px; color: white; margin-top: 10px;"></ion-icon></router-link>

        </b-navbar>
    </div>
</template>

<script>
    import liff from '@line/liff'
    export default {
        name: "NavMenu",
        data() {
            return {
                isExpanded: false,
                fromText: "",

            }
        },
        methods: {
            collapse() {
                this.isExpanded = false;
            },
            onClickBackBtn() {
                var routeName = this.$route.name
                if (routeName == "Accident") {
                    this.$router.push({ name: 'Advice' })
                } else if (routeName == "AccidentCreate") {
                    this.$router.push({ name: 'Accident' })
                } else if (routeName == "Rights") {               
                    this.$router.push({ name: 'Accident' })
                } else if (routeName == "RightsHistory") {
                    this.$router.push({ name: 'Rights' })
                } else if (routeName == "RightsHistoryDetail") {
                    this.$router.push({ name: 'RightsHistory', params: { id: this.$route.params.id, typerights: this.$route.params.typerights } })
                } else if (routeName == "ApprovalCreate") {
                    this.$router.push({ name: 'Rights' })
                } else if (routeName == "ConfirmOTP") {
                    if (this.$route.params.from == "Create") {
                        this.fromText = "การส่งคำร้อง"
                    } else if (this.$route.params.from == "Edit") {
                        this.fromText = "การส่งเอกสารเพิ่มเติม"
                    } else if (this.$route.params.from == "CanselApproval") {
                        this.fromText = "การยกเลิกคำร้อง"
                    } else if (this.$route.params.from == "CreateUser") {
                        this.fromText = "การลงทะเบียน"
                    } else if (this.$route.params.from == "ConfirmMoney") {
                        this.fromText = "การยอมรับจำนวนเงิน"
                    }
                    this.$swal({
                        icon: 'question',
                        text: 'ท่านต้องการจะยกเลิก' + this.fromText + 'ใช่หรือไม่?',
                        /*title: 'คำเตือน',*/
                        /*footer: '<a href="">Why do I have this issue?</a>'*/
                        showCancelButton: false,
                        showDenyButton: true,
                        denyButtonText: "<a style='color: #5c2e91; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ไม่",
                        denyButtonColor: '#dad5e9',
                        confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ใช่",
                        confirmButtonColor: '#5c2e91',
                        willClose: () => {

                        }
                    }).then((result) => {

                        if (result.isConfirmed) {
                            if (this.$route.params.from == "Create") {
                                this.$router.push({ name: 'Rights' })
                            } else if (this.$route.params.from == "Edit") {
                                this.$router.push({ name: 'Accident' })
                            } else if (this.$route.params.from == "CanselApproval") {
                                this.$router.push({ name: 'Accident' })
                            } else if (this.$route.params.from == "CreateUser") {
                                this.$router.push({ name: 'Advice' })
                            } else if (this.$route.params.from == "ConfirmMoney") {
                                this.$router.push({ name: 'Accident' })
                            }

                        }
                    });

                    /*this.$router.push({ name: 'ApprovalCreate', params: { id: this.$route.params.id }})*/
                } else if (routeName == "Approvals") {
                    this.$router.push({ name: 'Accident' })
                } else if (routeName == "ConfirmMoney") {
                    this.$router.push({ name: 'Approvals', params: { id: this.$route.params.id } })
                } else if (routeName == "ApprovalDetail") {
                    this.$router.push({ name: 'Approvals', params: { id: this.$route.params.id } })
                } else if (routeName == "Ocr") {
                    if (liff.getOS() == "ios") {
                        this.$router.push({ name: 'Advice' })
                    } else {
                        liff.closeWindow()
                    }

                } else if (routeName == "Rating") {
                    this.$router.push({ name: 'Approvals', params: { id: this.$route.params.id } })
                } else if (routeName == "ApprovalEdit") {
                    this.$router.push({ name: 'Approvals', params: { id: this.$route.params.id } })
                }
            },

            toggle() {
                this.isExpanded = !this.isExpanded;
            }
        }
    }
</script>

<style>
    .navbar-digital-cliam {
        background-color: var(--main-color);
        /*background: -webkit-linear-gradient(var(--main-color), rgb(114 60 177));*/
        text-align: center;
        height: 50px;
        padding-left: 15px;
        padding-right: 20px;
        /*box-shadow: 0 1px 3px 0 rgba(0,0,0,.2);*/
    }

    .b-navbar-brand {
        background-color: white;
        /*background: linear-gradient(45deg, #c331ff, var(--main-color));*/
        -webkit-background-clip: text;
        -webkit-text-fill-color: transparent;
        font-size: 23px;
        font-weight: bold;
    }
</style>