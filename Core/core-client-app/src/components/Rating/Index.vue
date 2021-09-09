<template>
    <div class="container ">
        <h2 id="header2">ประเมินความพึงพอใจ</h2>
        <div align="left" style="margin-top: 15%; margin-bottom: 10px">
            <label>ความพึงพอใจในภาพรวมของระบบ</label>
        </div>
        <div>
            <star-rating :border-width="5"
                         :animate="true"
                         :star-size="45"
                         border-color="#d8d8d8"
                         :rounded-corners="true"
                         :show-rating="false"
                         :star-points="[23,2, 14,17, 0,19, 10,34, 7,50, 23,43, 38,50, 36,34, 46,19, 31,17]">
            </star-rating>
        </div>
        <div align="left" style="margin-top: 30%;">
            <label>ข้อเสนอแนะเพิ่มเติม</label>
        </div>
        <b-form-textarea id="textarea"
                         class="mt-0 mb-2"
                         v-model="comment"
                         placeholder=""
                         rows="4"
                         max-rows="7">
        </b-form-textarea>
        <div style="margin-top: 15%;">
            <button class="btn-confirm-money" type="button" @click="showSwal">ส่งแบบประเมิน</button>
        </div>
    </div>
</template>

<script>
    import StarRating from 'vue-star-rating'
    export default {
        name: "Rating",
        components: {
            StarRating
        },
        data() {
            return {
                rating: "No Rating Selected",
                currentRating: "No Rating",
                currentSelectedRating: "No Current Rating",
                boundRating: 3,
                comment: ''
            };
        },
        methods: {
            setRating: function (rating) {
                this.rating = "You have Selected: " + rating + " stars";
            },
            showCurrentRating: function (rating) {
                this.currentRating = (rating === 0) ? this.currentSelectedRating : "Click to select " + rating + " stars"
            },
            setCurrentSelectedRating: function (rating) {
                this.currentSelectedRating = "You have Selected: " + rating + " stars";
            },
            showSwal() {                
                this.$swal({
                    icon: 'success',
                    //text: 'ท่านสามารถติดตามผลดำเนินการได้ที่เมนูติดตามสถานะ',
                    title: 'ส่งแบบประเมินแล้ว',
                    /*footer: '<a href="">Why do I have this issue?</a>'*/
                    showCancelButton: false,
                    showDenyButton: false,
                    //denyButtonText: "<a style='color: #5c2e91; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                   // denyButtonColor: '#dad5e9',
                    confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                    confirmButtonColor: '#5c2e91',
                    willClose: () => {
                        this.$router.push({ name: 'Accident' })
                    }
                })
            },
        }

    };
</script>

<style>
    .vue-star-rating {
        display: flex;
        justify-content: center;
        align-items: center;
    }
</style>