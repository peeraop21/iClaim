<template>
    <div class="container">
        <loading :active.sync="isLoading"
                 :can-cancel="false"
                 color="#5c2e91"
                 loader="dots"
                 :is-full-page="true">
        </loading>
        <div class="row">
            <div class="col-12" align="center">
                <h2 id="header2">ติดตามสถานะคำร้อง</h2>
                <section class="mt-4">
                    <div v-if="isHasIclaimApprovalData == true" style="height: 95%; width: 100%;">
                        <div class="accordion" v-for="iclaimApp in iclaimApprovalData" :key="iclaimApp.appNo">
                            <b-overlay :show="iclaimApp.status == 0">
                                <div class="accordion-item" :id="'list' + iclaimApp.appNo">
                                    <a class="accordion-link" :href="'#list' + iclaimApp.appNo">
                                        <div>
                                            <p style="margin-bottom: 10px">
                                                <ion-icon name="newspaper-outline"></ion-icon>คำร้องที่: {{ iclaimApp.appNo }}
                                                <br>
                                                <ion-icon name="calendar-outline"></ion-icon>วันที่ยื่นคำร้อง: {{ iclaimApp.stringRegDate }} น.
                                                <br>
                                                <ion-icon name="options-outline"></ion-icon>สถานะคำร้อง: {{iclaimApp.appStatusName}}
                                            </p>
                                        </div>
                                        <div align="right" style="margin-top: -10px;">
                                            <div style="margin-top:-5px">
                                                <vs-button v-if="iclaimApp.status >= 5"
                                                           class="pdf-btn"
                                                           v-on:click="externalPagePDF(iclaimApp.appNo)"
                                                           icon
                                                           primary
                                                           flat>
                                                    PDF
                                                </vs-button>
                                                <router-link :to="{ name: 'ApprovalDetail', params: { id: iclaimApp.stringAccNo, appNo: iclaimApp.appNo}}">
                                                    <vs-button circle
                                                               icon
                                                               primary
                                                               flat>
                                                        <ion-icon name="information" style="margin: -2px -7px -2px 0px; font-size: 17px"></ion-icon>
                                                    </vs-button>
                                                </router-link>
                                            </div>
                                            <ion-icon name="chevron-down-outline" class="icon ion-md-add" style="margin-right: 6px; margin-top: 10px"></ion-icon>
                                        </div>
                                    </a>
                                    <div class="answer" style="padding-left:30px;">

                                        <p class="p-custom custom-p-status">สถานะ </p>

                                        <ul id="progress" v-for="status in iclaimApp.appStatus" :key="status.statusId">
                                            <li class="li-custom " :id="'liLbl' + status.statusId">
                                                <p v-if="status.statusDate != null" class="p-custom divider-p" style="font-size: 8px; position: absolute; left: -12px; margin-top: -1px;">{{status.statusDate}}</p>
                                                <p v-if="status.statusTime != null" class="p-custom divider-p" style="font-size: 10px; position: absolute; left: -1px; margin-top: 7px; ">{{status.statusTime}}</p>
                                                <div class="node " v-bind:class="{green:status.active, grey:!status.active}"></div>
                                                <p class="p-custom divider-p" v-bind:class="{pgreen:status.active}">{{status.statusName}}</p>
                                            </li>
                                            <li v-if="status.statusId < iclaimApp.appStatus.length + iclaimApp.appStatus.length" class="li-custom " :id="'verticalLine' + status.statusId">
                                                <div class="divider grey"></div>
                                            </li>

                                        </ul>
                                        <br />
                                    </div>
                                    <div style="text-align: center">
                                        <router-link class="btn-select" v-if="iclaimApp.status == 2" :to="{ name: 'ApprovalEdit', params: { id: iclaimApp.stringAccNo, appNo: iclaimApp.appNo }}" style="padding-right: 10px; padding-left: 10px">แก้ไข/แนบเอกสารเพิ่มเติม</router-link>
                                        <router-link class="btn-checked" v-if="iclaimApp.status == 4" :to="{ name: 'ConfirmMoney', params: { id: iclaimApp.stringAccNo, appNo: iclaimApp.appNo}}">รายละเอียดการพิจารณา</router-link>
                                        <router-link class="btn-checked" v-if="iclaimApp.status == 9" :to="{ name: 'Rating', params: { id: iclaimApp.stringAccNo }}">ประเมินความพึงพอใจ</router-link>
                                    </div>
                                    <div style="text-align: center">

                                    </div>
                                </div>
                                <template #overlay>
                                    <div class="text-center">
                                        <p id="cancel-label" style="color:indianred; font-size:18px">คำร้องนี้ถูกยกเลิกแล้ว</p>
                                    </div>
                                </template>
                            </b-overlay>
                        </div>
                    </div>
                    <p v-if="isHasIclaimApprovalData == false" class="notData">- ไม่มีคำร้อง -</p>
                </section>
            </div>
        </div>
        <br>
    </div>
</template>

<style>
    .pdf-btn {
        box-shadow: 3px 3px 5px -4px #888888;
    }
    .verticalLine {
        border-left: 1px solid #bbb;
        margin-left: 25px;
    }

    *, *:after, *:before {
        margin: 0;
        padding: 0;
    }

    #progress {
        margin-bottom: 0rem;
        height: 24px;
    }

    .node {
        height: 10px;
        width: 10px;
        border-radius: 50%;
        display: inline-block;
        transition: all 1000ms ease;
    }

    .answer .p-custom {
        color: none;
        padding: 0rem 0 0 2rem;
    }

    .activated {
        box-shadow: 0px 0px 3px 2px rgba(194, 255, 194, 0.8);
    }

    .pgreen {
        color: rgba(92, 184, 92, 1);
    }

    .divider {
        height: 20px;
        width: 2px;
        margin-left: 4px;
        transition: all 800ms ease;
    }

    .divider-p {
        display: inline-block;
        margin-left: -10px;
        margin-bottom: 0px;
    }

    .li-custom {
        list-style: none;
        line-height: 1px;
    }

    .custom-p-status {
        color: black;
        margin-bottom: 5px;
        margin-top: 5px;
    }

    .blue {
        background-color: rgba(82, 165, 255, 1);
    }

    .green {
        background-color: rgba(92, 184, 92, 1);
    }

    .red {
        background-color: rgba(255, 148, 148, 1);
    }

    .grey {
        background-color: rgba(201, 201, 201, 1);
    }
</style>
<script>
    import mixin from '../../mixin/index.js'
    import liff from '@line/liff'
    import axios from 'axios'
    import Loading from 'vue-loading-overlay';
    export default {
        name: 'Accident',
        mixins: [mixin],
        components: {
            Loading
        },
        data() {
            return {

                userData: this.$store.state.userStateData,
                accData: this.$store.getters.accGetter(this.$route.params.id),
                iclaimApprovalData: null,
                isHasIclaimApprovalData: false,
                appStatus: [{ statusId: 0, statusName: "", active: false, statusDate: "", statusTime: "" }],
                isActive: true,
                pdfsrc: null,
                isLoading: true,


            }

        },
        methods: {

            getIclaimApproval() {
                this.isLoading = true             
                var url = this.$store.state.envUrl + '/api/Approval/IclaimApproval';
                const body = {
                    AccNo: this.$route.params.id,
                    VictimNo: parseInt(this.accData.victimNo),
                };
                var apiConfig = {
                    headers: {
                        'Authorization': "Bearer " + this.$store.state.jwtToken.token,
                        'Content-Type': 'application/json',
                    }
                }
                axios.post(url, JSON.stringify(body), apiConfig)
                    .then((response) => {
                        /*this.userApi = response.data;*/
                        this.$store.state.hosAppStateData = response.data;
                        this.iclaimApprovalData = this.$store.state.hosAppStateData;
                        var appNoDocumentNotPass = [];
                        var appNoConfirmMoney = [];
                        var showNoti = false;
                        for (let i = 0; i < this.iclaimApprovalData.length; i++) {
                            if (this.iclaimApprovalData[i].status == 2) {
                                appNoDocumentNotPass.push(this.iclaimApprovalData[i].appNo);
                                showNoti = true;
                            }
                            if (this.iclaimApprovalData[i].status == 4) {
                                appNoConfirmMoney.push(this.iclaimApprovalData[i].appNo);
                                showNoti = true;
                            }
                        }
                        if (showNoti) {
                            var htmlMessage = "";
                            if (appNoDocumentNotPass.length > 0) {
                                htmlMessage = htmlMessage + '<p align="left"> <strong>คำร้องที่ : ' + appNoDocumentNotPass + ' </strong><br>&emsp;&emsp;เอกสารของท่านไม่สมบูรณ์ กรุณากดที่ปุ่ม <label style="color:var(--main-color)">"แก้ไข/แนบเอกสารเพิ่มเติม"</label> เพื่อดูรายละเอียด และแก้ไขข้อมูล/แนบเอกสารที่ไม่สมบูรณ์</p>'
                            }
                            if (appNoConfirmMoney.length > 0) {
                                htmlMessage = htmlMessage + '<p align="left"> <strong>คำร้องที่ : ' + appNoConfirmMoney + ' </strong><br>&emsp;&emsp;เนื่องจากจำนวนเงินที่เจ้าหน้าที่พิจารณา ไม่ตรงตามจำนวนเงินที่ท่านส่งคำร้องขอไป กรุณากดที่ปุ่ม <label style="color:var(--main-color)">"รายละเอียดการพิจารณา"</label> เพื่อดูรายละเอียด และยอมรับจำนวนเงิน</p>'

                            }

                            this.$swal({
                                icon: 'info',
                                html: htmlMessage,
                                title: 'แจ้งเตือน',
                                showCancelButton: false,
                                showDenyButton: false,
                                confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                                confirmButtonColor: '#5c2e91',
                                willClose: () => {

                                }
                            })
                        }
                        if (this.iclaimApprovalData.length == 0) {
                            this.isHasIclaimApprovalData = false
                        } else if (this.iclaimApprovalData.length > 0) {
                            this.isHasIclaimApprovalData = true
                        }
                        this.isLoading = false
                    })
                    .catch((error) => {
                        if (error.toString().includes("401")) {
                            this.getJwtToken()
                            this.getIclaimApproval()
                        }
                        this.isLoading = false

                    });

            },
            externalPagePDF(appNo) {            
                var encodedString = window.btoa(this.$store.state.userTokenLine + '|' + this.$route.params.id + '|' + this.accData.victimNo + '|' + appNo);
                liff.openWindow({
                    url: location.origin + '/DownloadPDF?pdf=' + encodedString,
                    external: true
                })       
            },
        },
        async created() {
            await this.getIclaimApproval();

        },
    }
</script>