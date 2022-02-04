<template>
    <div class="container">
        <loading :active.sync="isLoading"
                 :can-cancel="false"
                 color="#5c2e91"
                 loader="dots"
                 :is-full-page="true">                   
        </loading>
        <div class="row">
            <div align="center">
                <h2 id="header2">ข้อมูลการรับแจ้งเหตุ</h2>
                <br>
                <div class="tab-user">
                    <div class="row">
                        <div class="col-3 px-0">
                            <img src="@/assets/kid.png" width="70">
                        </div>
                        <div class="col-9 text-start mt-2 px-0">
                            <span>ชื่อ-สกุล: <span style="color: var(--main-color)">{{userData.prefix}}{{userData.fname}} {{userData.lname}}</span></span>
                            <br>
                            <span>เลขประจำตัวประชาชน: <span style="color: var(--main-color)">{{userData.idcardNo}}</span></span>
                        </div>
                    </div>
                </div>
                <div class="mb-4 mt-4">
                    <button @click="testDOPAapi" hidden class="btn-next" to="">Test DOPA API</button>
                </div>

                <div align="left" style="width: 100%;">
                    <label class="title-advice-menu">ประวัติการแจ้งอุบัติเหตุ</label>
                </div>
                <div class="row mt-2 mb-2">
                    <div class="col-1">
                        
                    </div>
                    <div class="col-10">
                        <v-date-picker v-model="dateSearch" class="" locale="th" mode="date" :max-date='new Date()' :attributes='attrs' :model-config="dateModelConfig">
                            <template v-slot="{ inputValue, inputEvents }">
                                <!--<input class=" mt-0 mb-2 form-control "
                                       style="text-align:center"
                                       :value="inputValue"
                                       v-on="inputEvents"
                                       placeholder="ค้นหาด้วยวันที่เกิดเหตุ"
                                       readonly />-->
                                <b-input-group style="background-color: #e1deec; border-radius: 7px;">
                                    <b-input-group-prepend >                                        
                                        <b-button style=" border: none;" variant="outline-secondary" disabled><b-icon icon="search" /></b-button>                                     
                                    </b-input-group-prepend>
                                    <b-form-input style="text-align: center; background-color: #e1deec; border: none;"
                                                  :value="inputValue"  
                                                  v-on="inputEvents"
                                                  placeholder="ค้นหาด้วยวันที่เกิดเหตุ"
                                                  readonly
                                                  >
                                    </b-form-input>
                                    <b-input-group-append >
                                        <b-button @click="clearDateSearch" style=" border: none;" variant="outline-secondary" ><b-icon icon="x" /></b-button>
                                    </b-input-group-append>
                                </b-input-group>
                            </template>
                        </v-date-picker>

                    </div>
                    <div class="col-1">                        
                    </div>

                </div>
                <section>
                    <div style="height: 98%; width: 100%;">
                        <div v-for="accident in accData" v-bind:key="accident.stringAccNo">
                            <div v-if="accident.stringAccDate.startsWith(dateSearch) || dateSearch == null" class="accordion">
                                <div class="accordion-item" :id="'list' + accident.stringAccNo">
                                    <a class="accordion-link" :href="'#list' + accident.stringAccNo">
                                        <div>
                                            <p>
                                                <label>
                                                    <ion-icon name="newspaper-outline"></ion-icon>เลขที่รับแจ้ง: {{ accident.accNo }}
                                                </label>
                                                <br>
                                                <label v-if="accident.lastClaim.claimNo">
                                                    <ion-icon name="checkbox-outline"></ion-icon>เลขที่เคลม: {{ accident.lastClaim.claimNo }}
                                                </label>
                                                <label v-if="!accident.lastClaim.claimNo">
                                                    <ion-icon name="checkbox-outline"></ion-icon>เลขที่เคลม: ยังไม่เปิดเคลม
                                                </label>
                                                <br>
                                                <label>
                                                    <ion-icon name="calendar-outline"></ion-icon>วันที่เกิดเหตุ: {{ accident.stringAccDate }}
                                                </label>
                                            </p>
                                        </div>
                                        <ion-icon name="chevron-down-outline" class="icon ion-md-add"></ion-icon>
                                    </a>
                                    <div class="answer">
                                        <p>
                                            ทะเบียนรถ:
                                            <label v-for="(car, index) in accident.car" :key="`car-${index}`">{{car}}&nbsp;</label>
                                            <br />
                                            <label v-if="accident.cureRightsBalance >= 0">สิทธิ์ค่ารักษาเบื้องต้นคงเหลือ: {{ accident.cureRightsBalance }} บาท</label>
                                            <label v-if="accident.cureRightsBalance < 0">สิทธิ์ค่ารักษาเบื้องต้นคงเหลือ: 0 บาท</label>
                                            <br />

                                            <label v-if="accident.crippledRightsBalance >= 0">สิทธิ์ค่าสูญเสียอวัยวะคงเหลือ: {{ accident.crippledRightsBalance }} บาท</label>
                                            <label v-if="accident.crippledRightsBalance < 0">สิทธิ์ค่าสูญเสียอวัยวะคงเหลือ: 0 บาท</label>

                                            <br />
                                        </p>
                                    </div>
                                    <div style="text-align: center">
                                        <router-link class="btn-select" :to="{ name: 'Rights', params: { id: accident.stringAccNo}}">ใช้สิทธิ์</router-link>
                                        <router-link v-if="accident.countHosApp > 0" class="btn-checked" :to="{ name: 'Approvals', params: { id: accident.stringAccNo}}">ติดตามสถานะ<span v-if="accident.countNotify > 0" class="icon-count-notify">{{accident.countNotify}}</span></router-link>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        </div>
        <br>
    </div>
</template>

<script>
    import axios from 'axios'
    // Import component
    import Loading from 'vue-loading-overlay';
    // Import stylesheet
    import 'vue-loading-overlay/dist/vue-loading.css';

    export default {
        name: 'Accident',
        components: {
            Loading
        },
        data() {
            return {
                attrs: [
                    {
                        key: 'today',
                        dot: true,
                        dates: new Date(),
                    },
                ],
                dateModelConfig: {
                    type: 'string',
                    mask: 'DD/MM/YYYY',
                },
                dateSearch:null,
                userData: [],
                accData: [],
                rights_amount: 0,
                isLoading: true
            }
        },


        methods: {
            testDOPAapi() {
                var urlTest = this.$store.state.envUrl + '/api/DOPAtest'
                axios.get(urlTest)
                    .then((response) => {
                        const body = {
                            client_code: "A62",
                            key_name: "A62_key.pub",
                            request_time: response.data.time,
                            signature: response.data.signature
                        };
                        console.log(JSON.stringify(body))

                        axios.post("https://digitalgatewaytest.digital-access.com/api/auth/token", JSON.stringify(body), {
                            headers: {
                                'Content-Type': 'application/json',
                            }
                        }).then((response1) => {
                            alert(response1.data)

                        }).catch((error) => {
                            alert(error)

                        });
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            clearDateSearch() {
                this.dateSearch = null
            },
            getJwtToken() {
                var urlJwt = this.$store.state.envUrl + '/api/jwt'
                axios.post(urlJwt, {
                    Name: 'Nior',
                    Email: 'peeran@rvp.co.th'
                }).then((response) => {
                    this.$store.state.jwtToken = response.data
                    this.getAccidents();
                    this.getUser();
                }).catch(function (error) {
                    alert(error)
                })
            },
            getAccidents() {
                var url = this.$store.state.envUrl + '/api/accident/{userToken}'.replace('{userToken}', this.$store.state.userTokenLine);
                var tokenJwt = this.$store.state.jwtToken.token
                var apiConfig = {
                    headers: {
                        Authorization: "Bearer " + tokenJwt
                    }
                }
                axios.get(url, apiConfig)
                    .then((response) => {
                        this.$store.state.accStateData = response.data;
                        this.accData = this.$store.state.accStateData
                        this.isLoading = false
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getUser() {
                var url = this.$store.state.envUrl + '/api/user/{userToken}'.replace('{userToken}', this.$store.state.userTokenLine);
                var tokenJwt = this.$store.state.jwtToken.token
                var apiConfig = {
                    headers: {
                        Authorization: "Bearer " + tokenJwt
                    }
                }
                axios.get(url, apiConfig)
                    .then((response) => {
                        if (response.data == null) {
                            this.$router.push({ name: 'Advice' })
                        }
                        this.$store.state.userStateData = response.data;                       
                        this.userData = this.$store.state.userStateData;
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            
            
        },

        async mounted() {
            /*this.$store.state.userTokenLine = "FrameMock"*/
            await this.getJwtToken();
            
        }
    }
</script>

<style>
    .icon-count-notify {
        display: inherit;
        position: absolute;
        color:white;
        background-color: red;
        border: 1px solid #000;
        border-radius: 10px;
        width: 20px;
        height: 20px;
        margin-top: -10px;
    }
    #header2 {
        font-size: 18px;
        font-weight: bold;
    }

    p.p_right {
        text-align: right;
        width: 99%;
        font-weight: bold;
    }

    .txt {
        border-radius: 10px;
        border: 1px solid #cccccc;
        /*height: 300px; */
        padding: 25px 25px 15px 25px;
        width: 100%;
        text-align: left;
        line-height: 15px;
    }

    .tab-user {
        cursor: pointer;
        position: relative;
        padding: 1rem 1rem;
        border-radius: 1rem;
        line-height: 1.5rem;
        font-size: 12px;
        font-weight: 600;
        border: 1px solid #ddd;
        background-image: linear-gradient(-180deg, #FFFFFF 0%, #FFFFFF 50%);
        /*box-shadow: 0 0rem 0.5rem 0 #f5f5f5 inset, 0 -0.25rem 1.5rem #f5f5f5 inset, 0 0.75rem 0.5rem #f5f5f5 inset, 0 0.2rem 0.5rem 0 #f1f1f1 inset;*/
    }

        .tab-user.span {
            color: black;
            background-image: linear-gradient(0deg, #FFFFFF 0%, #FFFFFF 10%);
            -webkit-background-clip: content-box;
            background-clip: content-box;
        }

    /*.tab-user::before {
            content: "";
            display: block;
            height: 0.25rem;
            position: absolute;
            top: 0.5rem;
            left: 50%;
            transform: translateX(-50%);
            width: calc(100% - 7.5rem);
            background: #fff;
            border-radius: 100%;
            opacity: 0.8;
            background-image: linear-gradient(-270deg, #EEEEEE 0%, #FFFFFF 20%, #FFFFFF 80%, #EEEEEE 100%);
        }*/
    .mybtn {
        position: center;
        margin-left: 0px;
        justify-content: space-between;
        display: inline-block;
    }

    a[class="btn-next"]:link, a[class="btn-next"]:visited {
        background-color: var(--main-color);
        color: white;
        padding: 5px 50px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        font-size: 13px;
    }

    .btn-select {
        background-color: var(--main-color);
        color: white;
        padding: 85px 90px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        font-size: 13px;
        border: 2px solid #5c2e91;
        border-style: none;
    }

    .btn-next:link, .btn-next:visited {
        background-color: var(--main-color);
        color: white;
        padding: 5px 50px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        font-size: 15px;
    }

    .btn-next, .btn-next:active {
        background-color: #50287e;
        color: white;
    }


    a[class="btn-select"]:link, a[class="btn-select"]:visited {
        background-color: var(--main-color);
        margin-top: 10px;
        margin-left: 0px;
        color: white;
        padding: 4px 35px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        border-style: none;
        /*font-size: 14px;*/
    }


    a[class="btn-checked"]:link, a[class="btn-checked"]:visited {
        background-color: white;
        margin-top: 10px;
        margin-left: 10px;
        color: #5c2e91;
        padding: 3px 15px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        border-style: solid;
        border-color: var(--main-color);
        font-weight: bold;
        font-size: 12px;
    }

    a[class="btn-rightsHistory"]:link, a[class="btn-rightsHistory"]:visited {
        background-color: #dad5e9;
        color: var(--main-color);
        padding: 3px 20px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        font-weight: bold;
    }

    section {
        width: 100%;
        height: 70%;
        display: flex;
        align-Items: center;
        justify-content: center;
        text-align: left;
    }

    .accordion-item {
        background-color: white;
        border-radius: .4rem;
        border-color: #cccccc;
        margin-bottom: .5rem;
        padding: 1rem 1rem .5rem 1rem;
        border-bottom: 3.5px solid #bbbbbb;
    }

    .accordion-link {
        color: black;
        background-color: white;
        width: 100%;
        display: flex;
        align-Items: center;
        justify-content: space-between;
        padding: 0;
        line-height: 25px;
        text-decoration: none;
    }

        .accordion-link:hover {
            text-decoration: none;
            color: #5c2e91;
        }

        .accordion-link ion-icon {
            color: var(--main-color);
            padding: 0rem .5rem .1rem 0rem;
            vertical-align: middle;
            font-size: 18px;
        }
        /*.accordion-link .ion-md-remove {
        display: none;
    }*/
        .accordion-link .ion-md-add {
            margin-bottom: 5px;
            font-size: 16px;
        }

    .answer {
        max-height: 0;
        overflow: hidden;
        position: relative;
        background-color: #eeeeee;
        transition: max-height 650ms;
    }

        .answer::before {
            content: "";
            position: absolute;
            width: .3rem;
            height: 100%;
            background-color: var(--main-color);
            top: 50%;
            left: 0;
            transform: translateY(-50%);
        }

        .answer p {
            
            padding: 0.7rem 0 0 2rem;
        }

    .accordion-item:target .answer {
        max-height: 15rem;
    }

    .accordion-item:target .accordion-link .ion-md-add {
        display: none;
    }

    .box-container {
        border-radius: 10px;
        border: 1px solid #cccccc;
        padding: 20px 20px 15px 20px;
        text-align: left;
    }
</style>
